using SocialMediaLists.Domain.People;
using System.Collections.Generic;

namespace SocialMediaLists.Tests.Unit.Application.People.Data
{
    public static class PeopleData
    {
        public static IEnumerable<Person> GetSeedData()
        {
            return new List<Person>
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
        }
    }
}