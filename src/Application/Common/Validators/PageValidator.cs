using FluentValidation;
using SocialMediaLists.Application.Contracts.Common.Models;

namespace SocialMediaLists.Application.Common.Validators
{
    internal class PageValidator : AbstractValidator<PageModel>
    {
        internal PageValidator()
        {
            When(page => page.From.HasValue, () =>
            {
                RuleFor(page => page.From)
                    .GreaterThanOrEqualTo(0)
                    .WithMessage("Invalid start page");
            });
        }
    }
}