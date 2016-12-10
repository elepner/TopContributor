using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopContributor.Common.Model
{
    public class Project
    {
        public string Id { get; set; }
        public string SourceRepoId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public VCSRepository SourceRepository { get; set; }
    }
}
