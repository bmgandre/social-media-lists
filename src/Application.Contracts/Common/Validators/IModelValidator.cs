namespace SocialMediaLists.Application.Contracts.Common.Validators
{
    public interface IModelValidator<T>
    {
        void ValidateAndThrow(T entity);
    }
}