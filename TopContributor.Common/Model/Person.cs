using System.Collections.Generic;
using Newtonsoft.Json;

namespace TopContributor.Common.Model
{
    public class Person
    {
        public int Id {get; set;}

        public string FirstName {get; set;}

        public string LastName {get; set;}

        public string FullName{get; set;}

        public string PrimaryEmail { get; set; }

        [JsonIgnore]
        public List<Commit> Commits { get; set; }

        public List<RepoAccount> Accounts { get; set; }
    }
}