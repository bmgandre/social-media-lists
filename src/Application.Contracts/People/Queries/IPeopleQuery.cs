using SocialMediaLists.Application.Contracts.People.Models;
using System.Threading;
using System.Threading.Tasks;

namespace SocialMediaLists.Application.Contracts.People.Queries
{
    public interface IPeopleQuery
    {
        public Task<PersonModel> GetByIdAsync(long id, CancellationToken cancellationToken);
    }
}