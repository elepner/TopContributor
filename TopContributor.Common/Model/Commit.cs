using System;
using Newtonsoft.Json;

namespace TopContributor.Common.Model
{
    public class Commit
    {
        public string Id { get; set; }
        public VCSRepository Source { get; set; }

        /*Composite foreign key to reference author's repo accout*/
        public string VSCRepositoryId { get; set; }
        public string VSCAuthorAccountId { get; set; }

        [JsonIgnore]
        public RepoAccount AuthorRepoAccount { get; set; }

        public DateTime Created {get; set;}
        public string ProjectId { get; set; }
        public int Insertions {get; set;}
        public int Deletions {get; set;}
        public string Message { get; set; }
    }
}