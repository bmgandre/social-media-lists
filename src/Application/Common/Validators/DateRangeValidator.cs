using FluentValidation;
using SocialMediaLists.Application.Contracts.Common.Models;

namespace SocialMediaLists.Application.Common.Validators
{
    internal class DateRangeValidator : AbstractValidator<DateRangeModel>
    {
        internal DateRangeValidator()
        {
            When(dateRange => dateRange.Begin.HasValue && dateRange.End.HasValue, () =>
            {
                RuleFor(dateRange => dateRange.Begin)
                    .LessThanOrEqualTo(dateRange => dateRange.End)
                    .WithMessage("Invalid date range");
            });
        }
    }
}