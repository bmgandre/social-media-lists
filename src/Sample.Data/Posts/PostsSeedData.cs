using Bogus;
using SocialMediaLists.Domain.Posts;
using SocialMediaLists.Sample.Data.People;
using System.Collections.Generic;
using System.Linq;

namespace SocialMediaLists.Sample.Data.Posts
{
    public class PostsSeedData
    {
        private readonly PeopleSeedData _peopleSeedData;
        private readonly List<Post> _sample = new List<Post>();

        public PostsSeedData(PeopleSeedData peopleSeedData)
        {
            _peopleSeedData = peopleSeedData;
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
            var peopleData = _peopleSeedData.GetSample().ToList();
            var networks = new[] { "facebook", "twitter" };

            return new Bogus.Faker<Post>()
                .Rules((faker, post) =>
                {
                    post.Date = faker.Date.RecentOffset(180).DateTime;
                    post.Content = faker.WaffleText(1, false);
                    var person = faker.PickRandom(peopleData);
                    var account = faker.PickRandom(person.Accounts);
                    post.Network = account.Network;
                    post.Link = faker.Internet.UrlWithPath("https", $"{account.Network}.com");
                    post.Author = account.AccountName;
                });
        }

        public IEnumerable<Post> GenerateSeedData()
        {
            var fakerPost = GetPostFaker();
            var result = fakerPost.Generate(10000);
            _sample.AddRange(result.Take(100));

            return result;
        }
    }
}