using Bogus;
using SocialMediaLists.Domain.Posts;
using System.Collections.Generic;
using System.Linq;

namespace SocialMediaLists.Sample.Data.Posts
{
    public class PostsSeedData
    {
        private readonly List<Post> _sample = new List<Post>();

        public IReadOnlyCollection<Post> GetSample()
        {
            if (_sample.Count == 0)
            {
                _sample.AddRange(GetPostFaker().Generate(100));
            }
            return _sample;
        }

        private static Bogus.Faker<Post> GetPostFaker()
        {
            var networks = new[] { "facebook", "twitter" };
            return new Bogus.Faker<Post>()
               .RuleFor(post => post.Date, faker => faker.Date.RecentOffset(180).DateTime)
               .RuleFor(post => post.Network, faker => faker.PickRandom(networks))
               .RuleFor(post => post.Link, (faker, post) => faker.Internet.UrlWithPath("https", $"{post.Network}.com"))
               .RuleFor(post => post.Content, faker => faker.WaffleText(1, false));
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