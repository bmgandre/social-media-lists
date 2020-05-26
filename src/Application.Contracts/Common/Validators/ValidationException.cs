using System;

namespace SocialMediaLists.Application.Contracts.Common.Validators
{
    public class ValidationException : Exception
    {
        public ValidationException(string message, IValidationResult validationResult)
            : base(message)
        {
            ValidationResult = validationResult;
        }

        public IValidationResult ValidationResult { get; }
    }
}