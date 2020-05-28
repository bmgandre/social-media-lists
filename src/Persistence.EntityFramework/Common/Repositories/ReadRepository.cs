using Microsoft.EntityFrameworkCore;
using SocialMediaLists.Application.Contracts.Common.Data;
using System.Collections.Generic;
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
    }
}