using FluentValidation;
using SocialMediaLists.Application.Common.Validators;
using SocialMediaLists.Application.Contracts.People.Models;
using SocialMediaLists.Application.Contracts.People.Repositories;
using SocialMediaLists.Application.Contracts.People.Validators;
using System.Threading;
using System.Threading.Tasks;

namespace SocialMediaLists.Application.People.Validators
{
    public class PeopleFindValidators : IPeopleFindValidators
    {
        private readonly FluentPersonFindValidator _validator;

        public PeopleFindValidators(IReadPeopleRepository readPeopleRepository)
        {
            _validator = new FluentPersonFindValidator(readPeopleRepository);
        }

        public async Task ValidateAndThrowAsync(PersonFindModel entity, CancellationToken cancellationToken)
        {
            await _validator.ValidateConvertAndThrowAsync(entity, cancellationToken);
        }

        private class FluentPersonFindValidator : AbstractValidator<PersonFindModel>
        {
            private readonly IReadPeopleRepository _readPeopleRepository;

            public FluentPersonFindValidator(IReadPeopleRepository readPeopleRepository)
            {
                _readPeopleRepository = readPeopleRepository;

                RuleFor(personFindModel => personFindModel.PersonId)
                    .NotNull()
                    .GreaterThanOrEqualTo(1)
                    .MustAsync(BeAValidIdAsync);
            }

            private async Task<bool> BeAValidIdAsync(long id, CancellationToken cancellationToken)
            {
                var person = await _readPeopleRepository.FindAsync(cancellationToken, id);
                return person != null;
            }
        }
    }
}