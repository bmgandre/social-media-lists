using Bogus;
using SocialMediaLists.Domain.Posts;
using SocialMediaLists.Sample.Data.SocialLists;
using System.Collections.Generic;
using System.Linq;

namespace SocialMediaLists.Sample.Data.Posts
{
    public class PostsSeedData
    {
        private readonly SocialListSeedData _socialListSeedData;
        private readonly List<Post> _sample = new List<Post>();

        public PostsSeedData(SocialListSeedData socialListSeedData)
        {
            _socialListSeedData = socialListSeedData;
        }

        public IReadOnlyCollection<Post> GetSample()
        {
            if (_sample.Count == 0)
            {
                _sample.AddRange(GetPostFaker().Generate(100));
            }
            return _sample;
        }

        private Bogus.Faker<Post> GetPostFaker()
        {
            var socialListData = _socialListSeedData.GetSample().ToList();
            var networks = new[] { "facebook", "twitter" };

            return new Bogus.Faker<Post>()
                .Rules((faker, post) =>
                {
                    var socialList = faker.PickRandom(socialListData);
                    var socialListPerson = faker.PickRandom(socialList.SocialListPerson);
                    var account = faker.PickRandom(socialListPerson.Person.Accounts);

                    post.Date = faker.Date.RecentOffset(180).DateTime;
                    post.Content = faker.WaffleText(1, false);
                    post.Network = account.Network;
                    post.Link = $"https://{account.Network}.com/{account.AccountName}/{faker.Random.AlphaNumeric(20)}";
                    post.Author = account.AccountName;
                    post.Lists = new string[] { socialList.Name };
                });
        }

        public IEnumerable<Post> GenerateSeedData()
        {
            var fakerPost = GetPostFaker();
            var result = fakerPost.Generate(100000);
            _sample.AddRange(result.Take(100));

            return result;
        }
    }
}