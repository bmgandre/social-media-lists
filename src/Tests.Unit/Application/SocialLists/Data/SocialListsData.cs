using SocialMediaLists.Domain.People;
using SocialMediaLists.Domain.SocialLists;
using System.Collections.Generic;
using System.Linq;

namespace SocialMediaLists.Tests.Unit.Application.SocialLists.Data
{
    internal class SocialListsData
    {
        public static IEnumerable<SocialList> GetSeedData()
        {
            var people1 = new List<Person>
            {
                new Person
                {
                    PersonId = 1,
                    Name = "John",
                    Accounts = new List<SocialAccount> { new SocialAccount { AccoutName = "johnlennon", Network= "twitter", PersonId = 1 } }
                },
                new Person
                {
                    PersonId = 2,
                    Name = "Paul",
                    Accounts = new List<SocialAccount> { new SocialAccount { AccoutName = "paulmccartney", Network= "twitter", PersonId = 2 } }
                },
                new Person
                {
                    PersonId = 3,
                    Name = "George",
                    Accounts = new List<SocialAccount> { new SocialAccount { AccoutName = "georgeharrison", Network= "twitter", PersonId = 3 } }
                },
                new Person
                {
                    PersonId = 4,
                    Name = "Ringo",
                    Accounts = new List<SocialAccount> { new SocialAccount{ AccoutName = "ringostar", Network= "twitter", PersonId = 4 } }
                }
            };
            var list1 = new SocialList
            {
                SocialListId = 1,
                Name = "The Beatles",
                SocialListPerson = people1.Select(x => new SocialListPerson
                {
                    
                    People = x,
                    PersonId = x.PersonId
                }).ToList()
            };
            var list2 = new SocialList
            {
                SocialListId = 2,
                Name = "Paul McCartney and Wings",
                SocialListPerson = new SocialListPerson[]
                {
                    new SocialListPerson { People = people1[1], PersonId = people1[1].PersonId }
                }
            };

            return new SocialList[] { list1, list2 };
        }
    }
}