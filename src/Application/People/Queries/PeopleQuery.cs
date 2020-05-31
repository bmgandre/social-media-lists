using SocialMediaLists.Application.Common.Data;
using SocialMediaLists.Application.Contracts.People.Models;
using SocialMediaLists.Application.Contracts.People.Queries;
using SocialMediaLists.Application.Contracts.People.Repositories;
using SocialMediaLists.Application.Contracts.People.Validators;
using SocialMediaLists.Application.People.Specifications;
using SocialMediaLists.Domain.People;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SocialMediaLists.Application.People.Queries
{
    public class PeopleQuery : IPeopleQuery
    {
        private readonly IPeopleFindValidators _peopleFindValidators;
        private readonly IReadPeopleRepository _readPeopleRepository;

        public PeopleQuery(IReadPeopleRepository readPeopleRepository,
            IPeopleFindValidators peopleFindValidators)
        {
            _readPeopleRepository = readPeopleRepository;
            _peopleFindValidators = peopleFindValidators;
        }

        public async Task<PersonModel> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            await _peopleFindValidators.ValidateAndThrowAsync(new PersonFindModel { PersonId = id },
                cancellationToken);

            var specification = SpecificationBuilder<Person>.Create().WithId(id);
            var person = (await _readPeopleRepository.SearchAsync(specification, person => person.Accounts, cancellationToken))
                .First();
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