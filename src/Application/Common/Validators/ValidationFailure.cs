using SocialMediaLists.Application.Contracts.Common.Validators;

namespace SocialMediaLists.Application.Common.Validators
{
    internal class ValidationFailure : IValidationFailure
    {
        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }
    }
}