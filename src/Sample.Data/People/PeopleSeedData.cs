using SocialMediaLists.Domain.People;
using System.Collections.Generic;
using System.Linq;

namespace SocialMediaLists.Sample.Data.People
{
    public class PeopleSeedData
    {
        private readonly List<Person> _sample = new List<Person>();

        public IReadOnlyCollection<Person> GetSample()
        {
            if (_sample.Count == 0)
            {
                _sample.AddRange(GetPersonFaker().Generate(100));
            }
            return _sample;
        }

        internal static Bogus.Faker<Person> GetPersonFaker()
        {
            var networks = new[] { "facebook", "twitter" };

            return new Bogus.Faker<Person>()
                .Rules((faker, person) =>
                {
                    var fakerPerson = faker.Person;
                    person.Name = fakerPerson.FullName;
                    var accounts = Enumerable.Range(1, faker.Random.Int(1, 2)).Select(it =>
                    {
                        return new SocialAccount
                        {
                            AccountName = faker.Internet.UserName(fakerPerson.FirstName, fakerPerson.LastName),
                            Network = faker.PickRandom(networks)
                        };
                    });
                    person.Accounts = accounts.ToList();
                });
        }

        public IEnumerable<Person> GenerateSeedData()
        {
            var fakerPerson = GetPersonFaker();
            var result = fakerPerson.Generate(500);
            _sample.AddRange(result);

            return result;
        }
    }
}