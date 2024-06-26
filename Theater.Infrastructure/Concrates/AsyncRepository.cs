using Theater.Infrastructure.Abstracts;
using Theater.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Theater.Infrastructure.Concrates
{
    public class AsyncRepository<T> : IAsyncRepository<T>
            where T : class
    {
        protected readonly DbContext db;

        public AsyncRepository(DbContext db)
        {
            this.db = db;
        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await db.AddAsync(entity, cancellationToken);
            return entity;

        }

        public T Edit(T entity)
        {
            db.Entry(entity).State = EntityState.Modified;

            return entity;
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> expression = null)
        {
            var query = db.Set<T>().AsQueryable();

            if (expression is not null)
            {
                query = query.Where(expression);
            }

            return query;
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> expression = null, CancellationToken cancellationToken = default)
        {
            var query = db.Set<T>().AsQueryable();
            T? entity;

            if (expression is null)
            {
                entity = await query.FirstOrDefaultAsync(cancellationToken);
            }
            else
            {
                entity = await query.FirstOrDefaultAsync(expression, cancellationToken);
            }

            //if (entity is null)
            //    throw new NotFoundException($"{typeof(T).Name} not found");

            return entity;
        }

        public void Remove(T entity)
        {
            db.Remove(entity);
        }

        public Task<int> SaveAsync(CancellationToken cancellationToken = default)
        {
            return db.SaveChangesAsync(cancellationToken);
        }
    }
}
