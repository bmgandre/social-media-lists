using FluentValidation;
using SocialMediaLists.Application.Common.Validators;
using SocialMediaLists.Application.Contracts.Posts.Models;
using SocialMediaLists.Application.Contracts.Posts.Validators;
using System.Linq;

namespace SocialMediaLists.Application.Posts.Validators
{
    public class PostFilterValidator : IPostFilterValidator
    {
        private static readonly string[] _validateNetworks =
        {
            "facebook", "twitter"
        };

        private readonly FluentPostFilterValidator _validator = new FluentPostFilterValidator();

        public void ValidateAndThrow(PostFilter entity)
        {
            _validator.ValidateConvertAndThrow(entity);
        }

        private class FluentPostFilterValidator : AbstractValidator<PostFilter>
        {
            public FluentPostFilterValidator()
            {
                RuleFor(post => post.DateRange)
                    .SetValidator(new DateRangeValidator());

                RuleFor(post => post.Page)
                    .SetValidator(new PageValidator());

                When(post => !string.IsNullOrWhiteSpace(post.Network), () => {
                    RuleFor(post => post.Network)
                        .Must(network => _validateNetworks.Contains(network.ToLower()))
                        .WithMessage("Invalid network");
                });
            }
        }
    }
}