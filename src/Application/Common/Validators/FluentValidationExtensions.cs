using FluentValidation;
using SocialMediaLists.Application.Contracts.Common.Validators;
using System.Linq;

namespace SocialMediaLists.Application.Common.Validators
{
    internal static class FluentValidationExtensions
    {
        public static IValidationResult Convert(this FluentValidation.Results.ValidationResult result)
        {
            var converted = new ValidationResult
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(x => x.Convert()).ToList()
            };

            return converted;
        }

        public static IValidationFailure Convert(this FluentValidation.Results.ValidationFailure failure)
        {
            var converted = new ValidationFailure
            {
                ErrorMessage = failure.ErrorMessage,
                PropertyName = failure.PropertyName
            };

            return converted;
        }

        public static void ValidateConvertAndThrow<T>(this AbstractValidator<T> validator, T entity)
        {
            var result = validator.Validate(entity);
            if (!result.IsValid)
            {
                throw new Contracts.Common.Validators.ValidationException("Invalid filter", result.Convert());
            }
        }
    }
}