using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;

namespace SocialMediaLists.Persistence.EntityFramework.Common.Database
{
    public class SocialMediaListsContextFactory : IDesignTimeDbContextFactory<SocialMediaListsDbContext>
    {
        public static readonly ILoggerFactory ConsoleLoggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

        public SocialMediaListsDbContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<SocialMediaListsDbContext>()
                .UseSqlite($"Data Source={nameof(SocialMediaLists)}.db")
                .UseLoggerFactory(ConsoleLoggerFactory)
                .Options;

            return new SocialMediaListsDbContext(options);
        }
    }
}