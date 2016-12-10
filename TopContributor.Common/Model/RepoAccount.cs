using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TopContributor.Common.Model
{
    public class RepoAccount
    {
        public string SourceRepoId { get; set; }
        public string AccountId { get; set; }

        public string Email { get; set; }
        public string Name { get; set; }
    }
}
