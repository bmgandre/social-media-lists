using Microsoft.EntityFrameworkCore;
using SocialMediaLists.Application.Contracts.SocialLists.Repositories;
using SocialMediaLists.Domain.SocialLists;
using SocialMediaLists.Persistence.EntityFramework.Common.Repositories;

namespace SocialMediaLists.Persistence.EntityFramework.SocialLists.Repositories
{
    public class ReadSocialListsRepository : ReadRepository<SocialList>, IReadSocialListsRepository
    {
        public ReadSocialListsRepository(DbContext dbContext)
            : base(dbContext)
        {
        }
    }
}