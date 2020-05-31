using SocialMediaLists.Domain.SocialLists;
using SocialMediaLists.Sample.Data.People;
using System.Collections.Generic;
using System.Linq;

namespace SocialMediaLists.Sample.Data.SocialLists
{
    public class SocialListSeedData
    {
        private readonly List<SocialList> _sample = new List<SocialList>();

        public IReadOnlyCollection<SocialList> GetSample()
        {
            if (_sample.Count == 0)
            {
                _sample.AddRange(GetSocialListFaker().Generate(100));
            }
            return _sample;
        }

        private static Bogus.Faker<SocialList> GetSocialListFaker()
        {
            var fakerSocialListPerson = GetSocialListPersonFaker();

            return new Bogus.Faker<SocialList>()
                .Rules((faker, socialList) =>
                {
                    var memberCount = faker.Random.Int(5, 200);
                    socialList.SocialListPerson = fakerSocialListPerson.Generate(memberCount);
                    socialList.Name = faker.Company.CompanyName();
                });
        }

        private static Bogus.Faker<SocialListPerson> GetSocialListPersonFaker()
        {
            var fakerPerson = PeopleSeedData.GetPersonFaker();
            return new Bogus.Faker<SocialListPerson>()
                .RuleFor(socialListPerson => socialListPerson.People, faker => fakerPerson);
        }

        public IEnumerable<SocialList> GenerateSeedData()
        {
            var fakerSocialLists = GetSocialListFaker();
            var result = fakerSocialLists.Generate(10);
            _sample.AddRange(result.Take(5));

            return result;
        }
    }
}