
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TopContributor.Common.Crawler
{
    public interface IRepositoryReader
    {
        Task<CommitsQueryResult> QueryCommits(DateTime @from, DateTime to);
        Task<CommitDetails> GetCommitDetail(string commitId);
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

        /// <summary>
        /// If value is null <see cref="IRepositoryReader.GetAuthorDetail"/> will be called to get this.
        /// </summary>
        public Author Author { get; set; }

        public int Insertions { get; set; }
        public int Deletions { get; set; }
        public string Message { get; set; }
        public string ProjectName { get; set; }
        public DateTime Created { get; set; }
        public CommitDetails CommitDetails { get; set; }

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

    public class CommitDetails
    {
        public ICollection<FileInfo> AffectedFiles { get; set; }
    }

    public class FileInfo
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public int Insertions { get; set; }
        public int Deletions { get; set; }
    }
}
