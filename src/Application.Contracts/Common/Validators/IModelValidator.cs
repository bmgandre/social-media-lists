using System.Threading;
using System.Threading.Tasks;

namespace SocialMediaLists.Application.Contracts.Common.Validators
{
    public interface IModelValidator<T>
    {
        Task ValidateAndThrowAsync(T entity, CancellationToken cancellationToken);
    }
}