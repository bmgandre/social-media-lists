using SocialMediaLists.Application.Contracts.Common.Validators;
using System.Collections.Generic;

namespace SocialMediaLists.Application.Common.Validators
{
    internal class ValidationResult : IValidationResult
    {
        public bool IsValid { get; set; }

        public IList<IValidationFailure> Errors { get; set; }
    }
}