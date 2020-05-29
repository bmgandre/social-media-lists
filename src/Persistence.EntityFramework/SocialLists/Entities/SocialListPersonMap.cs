using Microsoft.EntityFrameworkCore;
using SocialMediaLists.Domain.SocialLists;

namespace SocialMediaLists.Persistence.EntityFramework.SocialLists.Entities
{
    internal static class SocialListPersonMap
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SocialListPerson>()
                .HasKey(entity => new
                {
                    entity.SocialListId,
                    entity.PersonId
                });

            modelBuilder.Entity<SocialListPerson>()
                .HasOne(entity => entity.SocialLists)
                .WithMany(socialList => socialList.SocialListPerson)
                .HasForeignKey(entity => entity.SocialListId);

            modelBuilder.Entity<SocialListPerson>()
                .HasOne(entity => entity.People)
                .WithMany(person => person.SocialListPerson)
                .HasForeignKey(entity => entity.PersonId);
        }
    }
}