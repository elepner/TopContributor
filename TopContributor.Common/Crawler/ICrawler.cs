
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TopContributor.Common.Crawler
{
    public interface ICrawler
    {
        Task<CommitsQueryResult> QueryCommits(DateTime @from, DateTime to);
        Task<CommitDetail> GetCommitDetail(string commitId);
        Task<Author> GetAuthorDetail(string authorId);
    }

    public struct CommitsQueryResult
    {
        public IEnumerable<Commit> Commits { get; set; }
        public bool QuerySizeExceeded { get; set; }
    }

    public class Commit
    {
        public string AuthorId { get; set; }
        public string Id { get; set; }
        public int Inserted { get; set; }
        public int Deletions { get; set; }
    }

    public class Author
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }

    public class CommitDetail
    {
        public IEnumerable<string> AffectedFiles { get; set; }
        public string CommitId { get; set; }
    }
}
