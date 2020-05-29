using Microsoft.EntityFrameworkCore;
using SocialMediaLists.Domain.People;
using SocialMediaLists.Domain.SocialLists;
using SocialMediaLists.Persistence.EntityFramework.SocialLists.Entities;

namespace SocialMediaLists.Persistence.EntityFramework.Common.Database
{
    public class SocialMediaListsDbContext : DbContext
    {
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<SocialList> SocialLists { get; set; }

        public SocialMediaListsDbContext(DbContextOptions<SocialMediaListsDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SocialListPersonMap.OnModelCreating(modelBuilder);
        }
    }
}