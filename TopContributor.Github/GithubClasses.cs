using System;
using System.Linq;
using Newtonsoft.Json;
using TopContributor.Common.Crawler;

namespace TopContributor.Github
{
    class GithubRepository
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    class GithubCommit
    {
        public string Sha { get; set; }
        public CommitInfo Commit { get; set; }
        public string Url { get; set; }
        public GithubUser Author { get; set; }

        public CommitStats Stats { get; set; }
        public FileStats[] Files { get; set; }

        public Commit ToModelCommit()
        {
            ;
            var authorId = Author != null ? Author.Login : Commit.Author.Email;
            var commit = new Commit()
            {
                Id = Sha,
                AuthorId = authorId,
                Message = Commit.Message,
                Created = Commit.Author.Date,
            };
            commit.CommitDetails = new CommitDetails();
            commit.Insertions = Stats.Additions;
            commit.Deletions = Stats.Deletions;
            commit.CommitDetails.AffectedFiles = Files.Select(x => new FileInfo
            {
                Deletions = x.Deletions,
                Insertions = x.Additions,
                Name = x.FileName
            }).ToArray();
            return commit;
        }
    }

    class GithubUser
    {
        public string Login { get; set; }
        public int Id { get; set; }
    }

    class CommitInfo
    {
        public GithubAuthor Author { get; set; }
        public string Message { get; set; }
    }

    class GithubAuthor
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }

    class CommitStats
    {
        public int Total;
        public int Additions;
        public int Deletions;
    }

    class FileStats
    {
        public string FileName;
        public string Status;
        public int Additions;
        public int Deletions;
        public int Changes;
    }

    class GithubProject
    {
        public string Name { get; set; }

        [JsonProperty("commits_url")]
        public string CommitsUrl { get; set; }

        [JsonProperty("created_at")]
        public DateTime Created { get; set; }

        [JsonProperty("updated_at")]
        public DateTime Updated { get; set; }
    }
}
