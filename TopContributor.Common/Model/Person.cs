using System.Collections.Generic;

namespace TopContributor.Common.Model
{
    public class Person
    {
        public string Id {get; set;}

        public string FirstName {get; set;}

        public string LastName {get; set;}

        public string FullName{get; set;}

        public List<string> Emails {get; set;}
    }
}