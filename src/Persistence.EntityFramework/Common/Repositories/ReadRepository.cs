﻿using Microsoft.EntityFrameworkCore;
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

        public virtual async Task<T> FindAsync(object[] keys, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Set<T>().FindAsync(keys, cancellationToken);
            return entity;
        }

        public virtual async Task<IEnumerable<T>> SearchAsync(ISpecification<T> specification,
            CancellationToken cancellationToken)
        {
            return await specification.Prepare(_dbContext.Set<T>().AsQueryable())
                .ToListAsync(cancellationToken);
        }

        public virtual async Task<IEnumerable<T>> SearchAsync<T1Property>(ISpecification<T> specification,
            Expression<Func<T, T1Property>> path1,
            CancellationToken cancellationToken)
        {
            var queryable = specification.Prepare(_dbContext.Set<T>().AsQueryable())
                .Include(path1);
            return await queryable.ToListAsync(cancellationToken);
        }

        public virtual IQueryable<T> Where<T1Property>(Expression<Func<T, bool>> predicate,
            Expression<Func<T, T1Property>> path1)
        {
            var queryable = _dbContext.Set<T>().AsQueryable().Include(path1);
            return queryable.Where(predicate);
        }

        public virtual IQueryable<T> Where<T1Property>(ISpecification<T> specification,
            Expression<Func<T, T1Property>> path1)
        {
            return Where(specification.Predicate, path1);
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

        public async Task<bool> ExistsAsync(ISpecification<T> specification, CancellationToken cancellationToken)
        {
            var count = await specification.Prepare(_dbContext.Set<T>().AsQueryable())
                .CountAsync(cancellationToken);
            return count > 0;
        }
    }
}