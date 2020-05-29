using Microsoft.EntityFrameworkCore;
using SocialMediaLists.Persistence.EntityFramework.Common.Database;
using System;

namespace SocialMediaLists.Tests.Unit.Persistence.EntityFramework.Common.Database
{
    internal sealed class MockDbContext
    {
        public MockDbContext()
        {
            var options = new DbContextOptionsBuilder<SocialMediaListsDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            DbContext = new SocialMediaListsDbContext(options);
        }

        public DbContext DbContext { get; }

        public void Dispose()
        {
            DbContext.Database.EnsureDeleted();
            DbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}