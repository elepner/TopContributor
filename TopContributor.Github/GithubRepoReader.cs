using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopContributor.Common.Crawler;

namespace TopContributor.Github
{
    public class GithubRepoReader : IRepositoryReader
    {
        public GithubRepoReader()
        {
        }

        public Task<CommitsQueryResult> QueryCommits(DateTime @from, DateTime to)
        {
            throw new NotImplementedException();
        }

        public Task<CommitDetail> GetCommitDetail(string commitId)
        {
            throw new NotImplementedException();
        }

        public Task<Author> GetAuthorDetail(string authorId)
        {
            throw new NotImplementedException();
        }

        public string Id { get; }
    }
}
