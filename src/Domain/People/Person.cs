using System.Collections.Generic;

namespace SocialMediaLists.Domain.People
{
    public class Person
    {
        public long PersonId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<SocialAccount> Accounts { get; set; }
    }
}