using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TopContributor.Gerrit
{
    internal class Project
    {
        public string Id { get; set; }
        public string Description { get; set; }
    }

    internal class Commit
    {
        public string Id { get; set; }

        [JsonProperty("owner")]
        public User Author { get; set; }

        [JsonProperty("subject")]
        public string Message { get; set; }

        [JsonProperty("project")]
        public string Project { get; set; }

        public DateTime Created { get; set; }

        public int Deletions { get; set; }

        public int Insertions { get; set; }
    }

    internal class User
    {
        [JsonProperty("_account_id")]
        public string Id { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }
    }
}
