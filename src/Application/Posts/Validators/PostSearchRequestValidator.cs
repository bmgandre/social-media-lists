using FluentValidation;
using SocialMediaLists.Application.Common.Data;
using SocialMediaLists.Application.Common.Validators;
using SocialMediaLists.Application.Contracts.People.Repositories;
using SocialMediaLists.Application.Contracts.Posts.Models;
using SocialMediaLists.Application.Contracts.Posts.Validators;
using SocialMediaLists.Application.Contracts.SocialLists.Repositories;
using SocialMediaLists.Application.People.Specifications;
using SocialMediaLists.Application.SocialLists.Specifications;
using SocialMediaLists.Domain.People;
using SocialMediaLists.Domain.SocialLists;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SocialMediaLists.Application.Posts.Validators
{
    public class PostSearchRequestValidator : IPostSearchRequestValidator
    {
        private static readonly string[] _validateNetworks =
        {
            "facebook", "twitter"
        };

        private readonly FluentPostFilterValidator _validator;

        public PostSearchRequestValidator(IReadSocialListsRepository readSocialListsRepository,
            IReadPeopleRepository readPeopleRepository)
        {
            _validator = new FluentPostFilterValidator(readSocialListsRepository, readPeopleRepository);
        }

        public async Task ValidateAndThrowAsync(PostSearchRequest entity, CancellationToken cancellationToken)
        {
            await _validator.ValidateConvertAndThrowAsync(entity, cancellationToken);
        }

        private class FluentPostFilterValidator : AbstractValidator<PostSearchRequest>
        {
            private readonly IReadPeopleRepository _readPeopleRepository;
            private readonly IReadSocialListsRepository _readSocialListsRepository;

            public FluentPostFilterValidator(IReadSocialListsRepository readSocialListsRepository,
                IReadPeopleRepository readPeopleRepository)
            {
                _readSocialListsRepository = readSocialListsRepository;
                _readPeopleRepository = readPeopleRepository;

                RuleFor(post => post.DateRange)
                    .SetValidator(new DateRangeValidator());

                RuleFor(post => post.Page)
                    .SetValidator(new PageValidator());

                When(post => !string.IsNullOrWhiteSpace(post.Network), () =>
                {
                    RuleFor(post => post.Network)
                        .Must(network => _validateNetworks.Contains(network.ToLower()))
                        .WithMessage("Invalid network");
                });

                When(post => post.Lists?.Count(x => !string.IsNullOrWhiteSpace(x)) > 0, () =>
                {
                    RuleForEach(post => post.Lists)
                        .MustAsync(BeAExistingListAsync)
                        .WithMessage("Invalid list");
                });

                When(post => post.AccountNames?.Count(x => !string.IsNullOrWhiteSpace(x)) > 0, () =>
                {
                    RuleForEach(post => post.AccountNames)
                        .MustAsync(BeAExistingAuthorAsync)
                        .WithMessage("Invalid account name");
                });
            }

            private async Task<bool> BeAExistingListAsync(string listName, CancellationToken cancellationToken)
            {
                var specification = SpecificationBuilder<SocialList>.Create()
                    .WithName(listName);
                var exists = await _readSocialListsRepository.ExistsAsync(specification, cancellationToken);
                return exists;
            }

            private async Task<bool> BeAExistingAuthorAsync(string accountName, CancellationToken cancellationToken)
            {
                var specification = SpecificationBuilder<Person>.Create()
                    .WithAccountName(accountName);
                var exists = await _readPeopleRepository.ExistsAsync(specification, cancellationToken);
                return exists;
            }
        }
    }
}