using System.Collections.Generic;

namespace SocialMediaLists.Application.Contracts.Common.Validators
{
    public interface IValidationResult
    {
        public bool IsValid { get; }
        public IList<IValidationFailure> Errors { get; }
    }
}