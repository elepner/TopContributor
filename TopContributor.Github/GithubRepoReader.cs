using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using TopContributor.Common.Crawler;

namespace TopContributor.Github
{
    public class GithubRepoReader : IRepositoryReader
    {
        private readonly GithubApiProvider _githubApiProvider;
        private readonly string _orgName;
        private JsonSerializer _jsonSerializer;

        private GithubProject[] _projects;

        public GithubRepoReader(GithubApiProvider githubApiProvider, string orgName)
        {
            _githubApiProvider = githubApiProvider;
            _orgName = orgName;
            _jsonSerializer = new JsonSerializer { ContractResolver = new CamelCasePropertyNamesContractResolver() };
        }
        
        public async Task<CommitsQueryResult> QueryCommits(DateTime @from, DateTime to)
        {
            
            CommitsQueryResult cqr = new CommitsQueryResult();
            
            if (_projects == null)
            {
                string reposListQuery = $"orgs/{_orgName}/repos";
                var response = await _githubApiProvider.ReadRequest(reposListQuery);
                JArray jArray = JArray.Parse(response);
                _projects = jArray.ToObject<GithubProject[]>(_jsonSerializer);
            }
            var queryTasks = _projects.Where(x => x.Created <= to)
                .Select(async x =>
                {
                    var commitsUrl = x.CommitsUrl.Replace(@"{/sha}", "");
                    var queryUrl = commitsUrl + $"?since={@from:O}&until={to:O}";
                    return new

                    {
                        Result = await _githubApiProvider.ReadRequest(queryUrl),
                        Repo = x
                    };
                });
            var results = await Task.WhenAll(queryTasks);

            var tasks = results.Select(r => new
                {
                    r.Repo,
                    Commits = JArray.Parse(r.Result).ToObject<GithubCommit[]>(_jsonSerializer)
                }).Select(obj =>
                {

                    var commits = obj.Commits.Select(c => new {obj.Repo.Name, c.Url});
                    return commits;
                })
                .SelectMany(x => x)
                .Select(async x =>
                {
                    var githubCommit =
                        JObject.Parse(await _githubApiProvider.ReadRequest(x.Url)).ToObject<GithubCommit>(_jsonSerializer);
                    var crawlerCommit = githubCommit.ToModelCommit();
                    crawlerCommit.ProjectName = x.Name;

                    crawlerCommit.Author = new Author
                    {
                        Id = crawlerCommit.AuthorId,
                        Email = githubCommit.Commit.Author.Email,
                        Name = githubCommit.Commit.Author.Name
                    };
                    return crawlerCommit;
                });


            cqr.Commits = await Task.WhenAll(tasks);
            return cqr;
        }

        public Task<CommitDetails> GetCommitDetail(string commitId)
        {
            throw new NotImplementedException();
        }

        public Task<Author> GetAuthorDetail(string authorId)
        {
            throw new NotImplementedException();
        }

        public string Id => "Github@" + _orgName;
    }
}
