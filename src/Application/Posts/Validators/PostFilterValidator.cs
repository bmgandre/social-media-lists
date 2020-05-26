using FluentValidation;
using SocialMediaLists.Application.Common.Validators;
using SocialMediaLists.Application.Contracts.Posts.Models;
using SocialMediaLists.Application.Contracts.Posts.Validators;
using System.Linq;

namespace SocialMediaLists.Application.Posts.Validators
{
    internal class PostFilterValidator : IPostFilterValidator
    {
        private static readonly string[] _validateNetworks =
        {
            "facebook", "twitter"
        };

        private readonly FluentPostFilterValidator _validator;

        public PostFilterValidator()
        {
            _validator = new FluentPostFilterValidator();
        }

        public void ValidateAndThrow(PostFilter entity)
        {
            var result = _validator.Validate(entity);
            if (!result.IsValid)
            {
                throw new Contracts.Common.Validators.ValidationException("Invalid filter", result.Convert());
            }
        }

        private class FluentPostFilterValidator : AbstractValidator<PostFilter>
        {
            public FluentPostFilterValidator()
            {
                RuleFor(post => post.DateRange.Begin)
                    .LessThanOrEqualTo(post => post.DateRange.End)
                    .WithMessage("Invalid date range");

                RuleFor(post => post.Page.From)
                    .GreaterThanOrEqualTo(0)
                    .WithMessage("Invalid start page");

                RuleFor(post => post.Network)
                    .Must(network => _validateNetworks.Contains(network.ToLower()))
                    .WithMessage("Invalid network");
            }
        }
    }
}