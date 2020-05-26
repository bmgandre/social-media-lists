namespace SocialMediaLists.Application.Contracts.Common.Validators
{
    public interface IValidationFailure
    {
        public string PropertyName { get; }
        public string ErrorMessage { get; }
    }
}