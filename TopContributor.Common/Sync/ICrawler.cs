using System;

namespace TopContributor.Common.Sync
{
    public interface ICrawler
    {
        
    }

    public class Commit 
    {
        public string AuthorId{ get; set; }
        public DateTime Created {get;set;}
    }
}