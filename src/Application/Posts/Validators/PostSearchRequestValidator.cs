using FluentValidation;
using SocialMediaLists.Application.Common.Data;
using SocialMediaLists.Application.Common.Validators;
using SocialMediaLists.Application.Contracts.Posts.Models;
using SocialMediaLists.Application.Contracts.Posts.Validators;
using SocialMediaLists.Application.Contracts.SocialLists.Repositories;
using SocialMediaLists.Application.SocialLists.Specifications;
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

        public PostSearchRequestValidator(IReadSocialListsRepository readSocialListsRepository)
        {
            _validator = new FluentPostFilterValidator(readSocialListsRepository);
        }

        public async Task ValidateAndThrowAsync(PostSearchRequest entity, CancellationToken cancellationToken)
        {
            await _validator.ValidateConvertAndThrowAsync(entity, cancellationToken);
        }

        private class FluentPostFilterValidator : AbstractValidator<PostSearchRequest>
        {
            private readonly IReadSocialListsRepository _readSocialListsRepository;

            public FluentPostFilterValidator(IReadSocialListsRepository readSocialListsRepository)
            {
                _readSocialListsRepository = readSocialListsRepository;

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

                    RuleForEach(post => post.Lists)
                        .MustAsync(BeANonEmptyListAsync)
                        .WithMessage("List is empty");
                });
            }

            private async Task<bool> BeAExistingListAsync(string listName, CancellationToken cancellationToken)
            {
                var specification = SpecificationBuilder<SocialList>.Create()
                    .WithName(listName);
                var exists = await _readSocialListsRepository.ExistsAsync(specification, cancellationToken);
                return exists;
            }

            private async Task<bool> BeANonEmptyListAsync(string listName, CancellationToken cancellationToken)
            {
                var specification = SpecificationBuilder<SocialList>.Create()
                    .WithName(listName)
                    .HasPeople();
                var isNonEmpty = await _readSocialListsRepository.ExistsAsync(specification, cancellationToken);
                return isNonEmpty;
            }
        }
    }
}