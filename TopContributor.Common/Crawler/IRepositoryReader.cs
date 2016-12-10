
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TopContributor.Common.Crawler
{
    public interface IRepositoryReader
    {
        Task<CommitsQueryResult> QueryCommits(DateTime @from, DateTime to);
        Task<CommitDetail> GetCommitDetail(string commitId);
        Task<Author> GetAuthorDetail(string authorId);
        string Id { get; }
    }

    public struct CommitsQueryResult
    {
        public IEnumerable<Commit> Commits { get; set; }
        public bool QuerySizeExceeded { get; set; }
    }

    public class Commit
    {
        public string Id { get; set; }
        public string AuthorId { get; set; }
        public int Insertions { get; set; }
        public int Deletions { get; set; }
        public string Message { get; set; }
        public string ProjectName { get; set; }
        public DateTime Created { get; set; }

        public override string ToString()
        {
            return
                $"Commit by author id: {AuthorId}, Message: {Message}, Insertions: {Insertions}, Deletions: {Deletions}";
        }
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
