using SocialMediaLists.Application.Contracts.Common.Data;
using SocialMediaLists.Domain.SocialLists;

namespace SocialMediaLists.Application.Contracts.SocialLists.Repositories
{
    public interface IReadSocialListsRepository : IReadRepository<SocialList>
    {
    }
}