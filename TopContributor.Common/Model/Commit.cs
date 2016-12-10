using System;

namespace TopContributor.Common.Model
{
    public class Commit
    {
        public string Id {get; set;}
        public DateTime Created {get; set;}
        public Person Author {get; set;}
        public int Inserted {get; set;}
        public int Deleted {get; set;}
    }
}