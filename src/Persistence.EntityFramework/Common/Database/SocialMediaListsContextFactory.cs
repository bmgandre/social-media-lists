using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SocialMediaLists.Persistence.EntityFramework.Common.Database
{
    public class SocialMediaListsContextFactory : IDesignTimeDbContextFactory<SocialMediaListsDbContext>
    {
        public SocialMediaListsDbContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<SocialMediaListsDbContext>()
                .UseSqlite($"Data Source={nameof(SocialMediaLists)}.db")
                .Options;

            return new SocialMediaListsDbContext(options);
        }
    }
}