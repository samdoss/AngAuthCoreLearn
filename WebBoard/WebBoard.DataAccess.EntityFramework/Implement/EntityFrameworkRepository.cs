using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebBoard.DataAccess.EntityFramework.Interface;

namespace WebBoard.DataAccess.EntityFramework.Implement
{
    public class EntityFrameworkRepository<T> : IEntityFrameworkRepository<T> where T : class
    {
        protected readonly DbContext _dbContext;
        public EntityFrameworkRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CountAsync()
        {
            return await _dbContext.Set<T>().CountAsync();
        }

        public virtual void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public virtual void Delete(List<T> entity)
        {
            _dbContext.Set<T>().RemoveRange(entity);
        }

        public virtual IQueryable<T> GetAll()
        {
            return _dbContext.Set<T>().AsQueryable();
        }

        public virtual IQueryable<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().Where(predicate).AsQueryable();
        }

        public IQueryable<T> GetAll(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query.AsQueryable();
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return await query.Where(predicate).FirstOrDefaultAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<T>> AddAsync(IEnumerable<T> entity)
        {
            await _dbContext.Set<T>().AddRangeAsync(entity);
            return entity;
        }

        public void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }
    }
}
