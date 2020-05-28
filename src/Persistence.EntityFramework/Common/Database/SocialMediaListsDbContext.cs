using Microsoft.EntityFrameworkCore;
using SocialMediaLists.Domain.People;

namespace SocialMediaLists.Persistence.EntityFramework.Common.Database
{
    public class SocialMediaListsDbContext : DbContext
    {
        public virtual DbSet<Person> People { get; set; }

        public SocialMediaListsDbContext(DbContextOptions<SocialMediaListsDbContext> options)
            : base(options)
        {
        }
    }
}