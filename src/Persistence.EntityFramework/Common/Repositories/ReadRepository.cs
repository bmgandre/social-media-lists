using Microsoft.EntityFrameworkCore;
using SocialMediaLists.Application.Contracts.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace SocialMediaLists.Persistence.EntityFramework.Common.Repositories
{
    public class ReadRepository<T> : IReadRepository<T> where T : class
    {
        private readonly DbContext _dbContext;

        public ReadRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<IEnumerable<T>> SearchAsync(ISpecification<T> specification,
            CancellationToken cancellationToken)
        {
            return await specification.Prepare(_dbContext.Set<T>().AsQueryable())
                .ToListAsync(cancellationToken);
        }

        public virtual IQueryable<T> Where<T1Property, T2Property>(Expression<Func<T, bool>> predicate,
            Expression<Func<T, IEnumerable<T1Property>>> path1, Expression<Func<T1Property, T2Property>> path2)
        {
            var queryable = _dbContext.Set<T>().AsQueryable().Include(path1).ThenInclude(path2);
            return queryable.Where(predicate);
        }

        public virtual IQueryable<T> Where<T1Property, T2Property>(ISpecification<T> specification,
            Expression<Func<T, IEnumerable<T1Property>>> path1, Expression<Func<T1Property, T2Property>> path2)
        {
            return Where(specification.Predicate, path1, path2);
        }
    }
}