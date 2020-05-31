using SocialMediaLists.Application.Contracts.SocialLists.Models;
using System.Threading;
using System.Threading.Tasks;

namespace SocialMediaLists.Application.Contracts.SocialLists.Queries
{
    public interface ISocialListsQuery
    {
        public Task<SocialListModel> GetByIdAsync(long id, CancellationToken cancellationToken);
    }
}