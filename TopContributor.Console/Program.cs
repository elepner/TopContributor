using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using TopContributor.Common;
namespace TopContributor.Console
{
    public class Program
    {
        private static readonly string GerritXssiProtectionPrefix = ")]}'";
        private static readonly string CommitsDumpFilePath = "./dump.json";
        private static readonly string AuthorsInfoDumpFilePath = "./authors.json";
        private static Dictionary<string, User> _usersCache = new Dictionary<string, User>();
        private static string _gerritUrl;
        private static string _gerritUser;
        private static string _gerritPwd;
        private static HttpClient _httpClient;
        private static bool _cacheUpdated = false;

        public static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                System.Console.WriteLine("Specify gerrit URL, gerrit user and gerrit HTTP password in the parameters.");
                System.Console.ReadKey();
                return;
            }

            _gerritUrl = args[0];
            _gerritUser = args[1];
            _gerritPwd = args[2];
            
            
            DoRequest().Wait();
            System.Console.ReadKey();
        }

        private static async Task DoRequest()
        {
            JArray commitsArray;

            var credCache = new CredentialCache();
            credCache.Add(new Uri(_gerritUrl), "Digest", new NetworkCredential(_gerritUser, _gerritPwd));
            _httpClient = new HttpClient(new HttpClientHandler { Credentials = credCache });

            if (File.Exists(CommitsDumpFilePath))
            {
                commitsArray = JArray.Parse(File.ReadAllText(CommitsDumpFilePath));
            }
            else
            {
                commitsArray = await LoadAllCommits();
            }

            System.Console.WriteLine($"Total number of gathered commits: {commitsArray.Count}");
            JsonSerializer jsonSerializer = new JsonSerializer();
            jsonSerializer.ContractResolver = new CamelCasePropertyNamesContractResolver();
            var commits = commitsArray.ToObject<List<Commit>>(jsonSerializer);
            var byId = commits.GroupBy(x => x.Id).Where(x => x.Count() >= 2).Select(x=>x.Key).ToArray();
            var byProject =
                commits.GroupBy(x => x.Project)
                    .Select(x => new {Name = x.Key, Count = x.Count()}).OrderByDescending(x => x.Count).Take(10);

            System.Console.WriteLine("Top 10 Users by Commit.");
            foreach (var topProjects in byProject)
            {
                System.Console.WriteLine($"Project name: {topProjects.Name}, Commits count: {topProjects.Count}");
            }

            LoadUsers();
            Dictionary<User, int> userStatistics = new Dictionary<User, int>();
            foreach (var author in commits.GroupBy(x => x.Author.Id).Select(x => new {Id = x.Key, Count = x.Count()}))
            {
                var user = await GetUserById(author.Id);
                if (user == null)
                {
                    System.Console.WriteLine($"User {author.Id} not found");
                    continue;
                }
                userStatistics.Add(user, author.Count);
            }

            var usersOrdered =
                userStatistics.Select(x => new {UserName = x.Key.Name, Email = x.Key.Email, Count = x.Value})
                    .GroupBy(x => x.UserName)
                    .Select(x => new {Name = x.Key, Count = x.Sum(y => y.Count)}).OrderByDescending(x => x.Count).Take(10);

            System.Console.WriteLine("Top 10 Users by Commit.");
            foreach (var topUsers in usersOrdered)
            {
                System.Console.WriteLine($"Name: {topUsers.Name}, Commits count: {topUsers.Count}");
            }

            if (_cacheUpdated)
            {
                var usersArray = _usersCache.Select(x => x.Value).ToArray();
                File.WriteAllText(AuthorsInfoDumpFilePath, JsonConvert.SerializeObject(usersArray));
            }
        }

        private static void LoadUsers()
        {
            if (File.Exists(AuthorsInfoDumpFilePath))
            {
                var jarray = JArray.Parse(File.ReadAllText(AuthorsInfoDumpFilePath));
                _usersCache = jarray.ToObject<List<User>>().ToDictionary(x => x.Id, x => x);
            }
        }

        private static async Task<User> GetUserById(string userId)
        {
            if (_usersCache.ContainsKey(userId))
            {
                return _usersCache[userId];
            }

            var userInfo = await MakeGetRequest($"accounts/{userId}/");
            JObject json = JObject.Parse(userInfo);
            JsonSerializer jsonSerializer = new JsonSerializer();
            jsonSerializer.ContractResolver = new CamelCasePropertyNamesContractResolver();
            _cacheUpdated = true;
            var user = json.ToObject<User>(jsonSerializer);
            System.Console.WriteLine($"New user {user.Name}/{user.Email} obtained.");
            _usersCache.Add(userId, user);
            return user;
        }

        private static async Task<JArray> LoadAllCommits()
        {
            
            var now = DateTime.Now.Date;

            var queries = Enumerable.Range(0, (4 * 12) * 2).Select(offset =>
            {
                return new[] { now - new TimeSpan(15 * offset, 0, 0, 0), now - new TimeSpan(15 * (offset + 1), 0, 0, 0) }; //Perhaps need to add 1 second here, but who makes commits at 12 am??
            }).Select(dates =>
            {
                string format = "yyyy-MM-dd";
                return new[] { dates[0].ToString(format), dates[1].ToString(format) };
            }).Select(dates => $"changes/?q=before:{dates[0]}+after:{dates[1]}");
            JArray finalArray = null;
            foreach (var query in queries)
            {
                var queryResult = await MakeGetRequest(query);
                var jarray = JArray.Parse(queryResult);
                if (jarray.Count == 0) break;
                System.Console.WriteLine($"Successfully obtained {jarray.Count} objects.");
                if (finalArray != null)
                {
                    finalArray.Merge(jarray);
                }
                else
                {
                    finalArray = jarray;
                }
            }
            if (finalArray != null)
            {
                
                File.WriteAllText(CommitsDumpFilePath, finalArray.ToString());
            }
            return finalArray;
        }

        private static async Task<string> MakeGetRequest(string gerritRequest)
        {
            var response = await _httpClient.GetAsync($"{_gerritUrl}/a/{gerritRequest}");
            var result = CleanUpResult(await response.Content.ReadAsStringAsync());
            
            return result;
        }

        private static string CleanUpResult(string result)
        {
            if (result.StartsWith(GerritXssiProtectionPrefix))
            {
                result = result.Replace(GerritXssiProtectionPrefix, "");
            }
            return result;
        }
    }
}
