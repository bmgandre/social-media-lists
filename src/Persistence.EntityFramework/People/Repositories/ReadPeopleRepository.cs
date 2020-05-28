using Microsoft.EntityFrameworkCore;
using SocialMediaLists.Domain.People;
using SocialMediaLists.Persistence.EntityFramework.Common.Repositories;

namespace SocialMediaLists.Persistence.EntityFramework.People.Repositories
{
    public class ReadPeopleRepository : ReadRepository<Person>
    {
        public ReadPeopleRepository(DbContext dbContext)
            : base(dbContext)
        {
        }
    }
}