﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using TopContributor.Common.Crawler;

namespace TopContributor.Gerrit
{
    public class GerritRepoReader : IRepositoryReader
    {
        private readonly IRequestProvider _repoReader;
        private readonly JsonSerializer _jsonSerializer;
        private static readonly string GerritXssiProtectionPrefix = ")]}'";

        public GerritRepoReader(IRequestProvider repoReader)
        {
            _repoReader = repoReader;
            _jsonSerializer = new JsonSerializer {ContractResolver = new CamelCasePropertyNamesContractResolver()};
        }

        public async Task<CommitsQueryResult> QueryCommits(DateTime @from, DateTime to)
        {
            string query = $"changes/?q=before:{to:yyyy-MM-dd}+after:{from:yyyy-MM-dd}";
            var result = await ReadGerritRequest(query);
            
            var gerritCommits = JArray.Parse(result).ToObject<List<Commit>>(_jsonSerializer);
            CommitsQueryResult commitsQueryResult = new CommitsQueryResult
            {
                Commits = gerritCommits.Select(gerritCommit =>
                {
                    var crawlerCommit = new Common.Crawler.Commit
                    {
                        AuthorId = gerritCommit.Author.Id,
                        Deletions = gerritCommit.Deletions,
                        Insertions = gerritCommit.Insertions,
                        Id = gerritCommit.Id,
                        Message = gerritCommit.Message,
                        Created = gerritCommit.Created,
                        ProjectName = gerritCommit.Project
                    };
                    return crawlerCommit;
                }),
                QuerySizeExceeded = gerritCommits.Count >= 500
            };
            return commitsQueryResult;
        }

        public Task<CommitDetails> GetCommitDetail(string commitId)
        {
            throw new NotImplementedException();
        }

        public async Task<Author> GetAuthorDetail(string authorId)
        {
            string query = $"accounts/{authorId}/";
            var result = await ReadGerritRequest(query);
            JObject json = JObject.Parse(result);
            var gerritUser = json.ToObject<User>(_jsonSerializer);
            return new Author
            {
                Email = gerritUser.Email,
                Id = gerritUser.Id,
                Name = gerritUser.Name
            };
        }

        public string Id => $"{_repoReader.GetType().FullName}@{_repoReader.Id}";

        private async Task<string> ReadGerritRequest(string query)
        {
            var result = await _repoReader.ReadRequest(query);
            if (result.StartsWith(GerritXssiProtectionPrefix))
            {
                result = result.Replace(GerritXssiProtectionPrefix, "");
            }
            return result;
        }
    }
}
