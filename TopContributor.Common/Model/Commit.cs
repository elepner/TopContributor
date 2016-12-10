using System;

namespace TopContributor.Common.Model
{
    public class Commit
    {
        public string Id { get; set; }
        public VCSRepository Source { get; set; }
        
        public Person Author { get; set; }
        public int AuthorId { get; set; }
        public DateTime Created {get; set;}
        public string ProjectId { get; set; }
        public int Insertions {get; set;}
        public int Deletions {get; set;}
        public string Message { get; set; }
    }
}