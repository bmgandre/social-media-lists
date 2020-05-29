using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace SocialMediaLists.Application.Contracts.Common.Data
{
    public interface IReadRepository<T>
    {
        Task<IEnumerable<T>> SearchAsync(ISpecification<T> specification, CancellationToken cancellationToken);

        IQueryable<T> Where<T1Property, T2Property>(Expression<Func<T, bool>> predicate,
            Expression<Func<T, IEnumerable<T1Property>>> path1,
            Expression<Func<T1Property, T2Property>> path2);

        IQueryable<T> Where<T1Property, T2Property>(ISpecification<T> specification,
            Expression<Func<T, IEnumerable<T1Property>>> path1,
            Expression<Func<T1Property, T2Property>> path2);

        Task<bool> ExistsAsync(ISpecification<T> specification, CancellationToken cancellationToken);
    }
}