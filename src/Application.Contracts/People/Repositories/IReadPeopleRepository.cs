using SocialMediaLists.Application.Contracts.Common.Data;
using SocialMediaLists.Domain.People;

namespace SocialMediaLists.Application.Contracts.People.Repositories
{
    public interface IReadPeopleRepository : IReadRepository<Person>
    {
    }
}