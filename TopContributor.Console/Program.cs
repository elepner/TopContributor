using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using TopContributor.Common.Crawler;
using TopContributor.Common.DataAccess;
using TopContributor.Gerrit;
using User = TopContributor.Common.User;
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
            var gerritRepoReader = new HttpGerritRepoReader(_gerritUrl, _gerritUser, _gerritPwd);
            var gerritReader = new GerritRepoReader(gerritRepoReader);


            var optionsBuilder = new DbContextOptionsBuilder<RepoDataContext>();

            var connection = @"Server=(localdb)\mssqllocaldb;Database=TopContributor.AspNetCore.NewDb;Trusted_Connection=True;";
            optionsBuilder.UseSqlServer(connection);


            using (var context = new RepoDataContext(optionsBuilder.Options))
            {
                var crawler = new RepoCrawler(context, gerritReader);
                crawler.SyncData();
            }
            
            //var result = await gerritCrawler.QueryCommits(DateTime.Now.Subtract(new TimeSpan(15, 0, 0, 0)), DateTime.Now);
            //foreach (var resultCommit in result.Commits)
            //{
            //    System.Console.WriteLine(resultCommit);
            //}
        }
        
    }
}
