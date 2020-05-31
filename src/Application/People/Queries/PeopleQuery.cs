using SocialMediaLists.Application.Contracts.People.Models;
using SocialMediaLists.Application.Contracts.People.Queries;
using SocialMediaLists.Application.Contracts.People.Repositories;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SocialMediaLists.Application.People.Queries
{
    public class PeopleQuery : IPeopleQuery
    {
        private readonly IReadPeopleRepository _readPeopleRepository;

        public PeopleQuery(IReadPeopleRepository readPeopleRepository)
        {
            _readPeopleRepository = readPeopleRepository;
        }

        public async Task<PersonModel> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            var person = await _readPeopleRepository.FindAsync(cancellationToken, id);
            return new PersonModel
            {
                Id = person.PersonId,
                Name = person.Name,
                Accounts = person.Accounts.Select(account =>
                {
                    return new SocialAccountModel
                    {
                        AccoutName = account.AccoutName,
                        Network = account.Network
                    };
                })
            };
        }
    }
}