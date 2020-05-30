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
            var fakerSocialAccount = GetSocialAccountFaker();

            return new Bogus.Faker<Person>()
                .RuleFor(person => person.Name, faker => faker.Person.FullName)
                .RuleFor(person => person.Accounts, faker => fakerSocialAccount.Generate(faker.Random.Int(1, 2)));
        }

        internal static Bogus.Faker<SocialAccount> GetSocialAccountFaker()
        {
            var networks = new[] { "facebook", "twitter" };
            return new Bogus.Faker<SocialAccount>()
                .RuleFor(account => account.AccoutName, faker => faker.Person.UserName)
                .RuleFor(account => account.Network, faker => faker.PickRandom(networks));
        }

        public IEnumerable<Person> GenerateSeedData()
        {
            var fakerPerson = GetPersonFaker();
            var result = fakerPerson.Generate(1000);
            _sample.AddRange(result.Take(50));

            return result;
        }
    }
}