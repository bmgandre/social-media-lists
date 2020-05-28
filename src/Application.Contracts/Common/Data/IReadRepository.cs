using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SocialMediaLists.Application.Contracts.Common.Data
{
    public interface IReadRepository<T>
    {
        Task<IEnumerable<T>> SearchAsync(ISpecification<T> specification, CancellationToken cancellationToken);
    }
}