using Microsoft.EntityFrameworkCore;
using Nest;
using SocialMediaLists.Domain.Posts;
using SocialMediaLists.Persistence.EntityFramework.Common.Database;
using SocialMediaLists.Sample.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SocialMediaLists.Sample.Data.Database
{
    public class SampleDataSeeder
    {
        private readonly DataSeedCollection _dataSeedCollection;
        private readonly SocialMediaListsDbContext _socialMediaListsDbContext;
        private readonly IElasticClient _elasticClient;

        public SampleDataSeeder(IElasticClient elasticClient,
            SocialMediaListsDbContext socialMediaListsDbContext,
            DataSeedCollection dataSeedCollection)
        {
            _elasticClient = elasticClient;
            _socialMediaListsDbContext = socialMediaListsDbContext;
            _dataSeedCollection = dataSeedCollection;
        }

        public async Task SeedAsync(CancellationToken cancellationToken)
        {
            if (await ShouldSeedAsync(cancellationToken))
            {
                await SeedSocialListsAsync(cancellationToken);
                await SeedPostsAsync(cancellationToken);
            }
        }

        private async Task<bool> ShouldSeedAsync(CancellationToken cancellationToken)
        {
            var hasValue = await _socialMediaListsDbContext.SocialLists.AnyAsync(cancellationToken);
            return !hasValue;
        }

        private async Task SeedSocialListsAsync(CancellationToken cancellationToken)
        {
            await _socialMediaListsDbContext.AddRangeAsync(_dataSeedCollection.People.GenerateSeedData(), cancellationToken);
            await _socialMediaListsDbContext.SaveChangesAsync(cancellationToken);

            await _socialMediaListsDbContext.AddRangeAsync(_dataSeedCollection.SocialList.GenerateSeedData(), cancellationToken);
            await _socialMediaListsDbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task SeedPostsAsync(CancellationToken cancellationToken)
        {
            await _elasticClient.IndexManyAsync(_dataSeedCollection.Posts.GenerateSeedData(),
                nameof(Post).ToLower(), cancellationToken);
        }

        public void Seed()
        {
            if (ShouldSeed())
            {
                SeedSocialLists();
                SeedPosts();
            }
        }

        private bool ShouldSeed()
        {
            var hasValue = _socialMediaListsDbContext.SocialLists.Any();
            return !hasValue;
        }

        private void SeedSocialLists()
        {
            _socialMediaListsDbContext.AddRange(_dataSeedCollection.People.GenerateSeedData());
            _socialMediaListsDbContext.SaveChanges();

            _socialMediaListsDbContext.AddRange(_dataSeedCollection.SocialList.GenerateSeedData());
            _socialMediaListsDbContext.SaveChanges();
        }

        private void SeedPosts()
        {
            _elasticClient.IndexMany(_dataSeedCollection.Posts.GenerateSeedData(),
                nameof(Post).ToLower());
        }
    }
}