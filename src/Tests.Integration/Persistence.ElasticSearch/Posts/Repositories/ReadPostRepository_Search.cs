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
        [Theory]
        [InlineData("Firefox", 3)]
        [InlineData("Chrome", 2)]
        [InlineData("Safari", 1)]
        public async Task Should_find_all_posts_by_the_content(string term, int expectCount)
        {
            var mockClient = new MockElasticClient<PostSource>(
                new[] {
                    new BaseIndexedEntry<PostSource> { id = Guid.NewGuid(), source = new PostSource { content = "Firefox" } },
                    new BaseIndexedEntry<PostSource> { id = Guid.NewGuid(), source = new PostSource { content = "Firefox" } },
                    new BaseIndexedEntry<PostSource> { id = Guid.NewGuid(), source = new PostSource { content = "Firefox" } },
                    new BaseIndexedEntry<PostSource> { id = Guid.NewGuid(), source = new PostSource { content = "Chrome" } },
                    new BaseIndexedEntry<PostSource> { id = Guid.NewGuid(), source = new PostSource { content = "Chrome" } },
                    new BaseIndexedEntry<PostSource> { id = Guid.NewGuid(), source = new PostSource { content = "Safari" } },
                }, "posts");
            var repository = new ReadPostRepository(mockClient.ElasticClient);

            var result = await repository.SearchAsync(new PostFilter(),
                CancellationToken.None);

            result.Count(x => x.Content == term).Should().Be(expectCount);
        }

        [Theory]
        [InlineData("Mozilla", 3)]
        [InlineData("Google", 2)]
        [InlineData("Apple", 1)]
        public async Task Should_find_all_posts_by_author(string author, int expectCount)
        {
            var mockClient = new MockElasticClient<PostSource>(
                new[] {
                    new BaseIndexedEntry<PostSource> { id = Guid.NewGuid(), source = new PostSource { author = "Mozilla" } },
                    new BaseIndexedEntry<PostSource> { id = Guid.NewGuid(), source = new PostSource { author = "Mozilla" } },
                    new BaseIndexedEntry<PostSource> { id = Guid.NewGuid(), source = new PostSource { author = "Mozilla" } },
                    new BaseIndexedEntry<PostSource> { id = Guid.NewGuid(), source = new PostSource { author = "Google" } },
                    new BaseIndexedEntry<PostSource> { id = Guid.NewGuid(), source = new PostSource { author = "Google" } },
                    new BaseIndexedEntry<PostSource> { id = Guid.NewGuid(), source = new PostSource { author = "Apple" } },
                }, "posts");
            var repository = new ReadPostRepository(mockClient.ElasticClient);

            var result = await repository.SearchAsync(new PostFilter(),
                CancellationToken.None);

            result.Count(x => x.Author == author).Should().Be(expectCount);
        }

        [Theory]
        [InlineData("2018-05-31", 3)]
        [InlineData("2019-05-31", 1)]
        [InlineData("2020-05-31", 2)]
        public async Task Should_find_all_posts_by_date(string dateStr, int expectCount)
        {
            var mockClient = new MockElasticClient<PostSource>(
                new[] {
                    new BaseIndexedEntry<PostSource> { id = Guid.NewGuid(), source = new PostSource { date = DateTime.Parse("2020-05-31") } },
                    new BaseIndexedEntry<PostSource> { id = Guid.NewGuid(), source = new PostSource { date = DateTime.Parse("2020-05-31") } },
                    new BaseIndexedEntry<PostSource> { id = Guid.NewGuid(), source = new PostSource { date = DateTime.Parse("2019-05-31") } },
                    new BaseIndexedEntry<PostSource> { id = Guid.NewGuid(), source = new PostSource { date = DateTime.Parse("2018-05-31") } },
                    new BaseIndexedEntry<PostSource> { id = Guid.NewGuid(), source = new PostSource { date = DateTime.Parse("2018-05-31") } },
                    new BaseIndexedEntry<PostSource> { id = Guid.NewGuid(), source = new PostSource { date = DateTime.Parse("2018-05-31") } },
                }, "posts");
            var repository = new ReadPostRepository(mockClient.ElasticClient);

            var result = await repository.SearchAsync(new PostFilter(),
                CancellationToken.None);

            result.Count(x => x.Date == DateTime.Parse(dateStr)).Should().Be(expectCount);
        }

        [Theory]
        [InlineData("twitter.com", 3)]
        [InlineData("facebook.com", 1)]
        [InlineData("linkedin.com", 2)]
        public async Task Should_find_all_posts_by_link(string link, int expectCount)
        {
            var mockClient = new MockElasticClient<PostSource>(
                new[] {
                    new BaseIndexedEntry<PostSource> { id = Guid.NewGuid(), source = new PostSource { link = "twitter.com" } },
                    new BaseIndexedEntry<PostSource> { id = Guid.NewGuid(), source = new PostSource { link = "linkedin.com" } },
                    new BaseIndexedEntry<PostSource> { id = Guid.NewGuid(), source = new PostSource { link = "twitter.com" } },
                    new BaseIndexedEntry<PostSource> { id = Guid.NewGuid(), source = new PostSource { link = "facebook.com" } },
                    new BaseIndexedEntry<PostSource> { id = Guid.NewGuid(), source = new PostSource { link = "twitter.com" } },
                    new BaseIndexedEntry<PostSource> { id = Guid.NewGuid(), source = new PostSource { link = "linkedin.com" } },
                }, "posts");
            var repository = new ReadPostRepository(mockClient.ElasticClient);

            var result = await repository.SearchAsync(new PostFilter(),
                CancellationToken.None);

            result.Count(x => x.Link == link).Should().Be(expectCount);
        }
    }
}