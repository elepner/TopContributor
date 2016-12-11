using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TopContributor.Common.Model
{
    public class RepoAccount
    {
        /*Composite primary key*/
        public string SourceRepoId { get; set; }
        public string AccountId { get; set; }

        public int PersonId { get; set; }
        public VCSRepository SourceRepository { get; set; }

        [JsonIgnore]
        public User User { get; set; }

        public string CommitId { get; set; }
        [JsonIgnore]
        public List<Commit> Commits { get; set; }

        public string Email { get; set; }
        public string Name { get; set; }
    }
}
