using FluentAssertions;
using SocialMediaLists.Application.Contracts.Posts.Models;
using SocialMediaLists.Persistence.ElasticSearch.Posts.Repositories;
using SocialMediaLists.Tests.Integration.Persistence.ElasticSearch.Common.Database;
using SocialMediaLists.Tests.Integration.Persistence.ElasticSearch.Common.Models;
using SocialMediaLists.Tests.Integration.Persistence.ElasticSearch.Posts.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SocialMediaLists.Tests.Integration.Persistence.ElasticSearch.Posts.Repositories
{
    public class ReadPostRepository_Search
    {
        private static BaseIndexedEntry<PostSource>[] GetSeedData()
        {
            return new BaseIndexedEntry<PostSource>[] {
                new BaseIndexedEntry<PostSource> { id = Guid.NewGuid(), source = new PostSource { content = "Firefox" } },
                new BaseIndexedEntry<PostSource> { id = Guid.NewGuid(), source = new PostSource { content = "Firefox" } },
                new BaseIndexedEntry<PostSource> { id = Guid.NewGuid(), source = new PostSource { content = "Firefox" } },
                new BaseIndexedEntry<PostSource> { id = Guid.NewGuid(), source = new PostSource { content = "Chrome" } },
                new BaseIndexedEntry<PostSource> { id = Guid.NewGuid(), source = new PostSource { content = "Chrome" } },
                new BaseIndexedEntry<PostSource> { id = Guid.NewGuid(), source = new PostSource { content = "Safari" } },
            };
        }

        [Theory]
        [InlineData("Firefox", 3)]
        [InlineData("Chrome", 2)]
        [InlineData("Safari", 1)]
        public async Task Should_find_all_posts_by_the_content(string term, int expectCount)
        {
            var mockClient = new MockElasticClient<PostSource>(GetSeedData(), "posts");
            var repository = new ReadPostRepository(mockClient.ElasticClient);

            var result = await repository.SearchAsync(new PostFilter(),
                CancellationToken.None);

            result.Count(x => x.Content == term).Should().Be(expectCount);
        }
    }
}