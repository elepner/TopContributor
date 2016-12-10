using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopContributor.Common.Model
{
    public class VCSRepository
    {
        public string Id { get; set; }
        public string CrawlerProviderType { get; set; }
        public string CrawlerParams { get; set; }

        public List<RepoAccount> Accounts { get; set; }
        public List<Project> Projects { get; set; }
    }
}
