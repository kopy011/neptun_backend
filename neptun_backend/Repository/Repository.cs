using Microsoft.EntityFrameworkCore;
using neptun_backend.Entities;
using System.Collections;
using System.Linq.Expressions;

namespace neptun_backend.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : AbstractEntity
    {
        protected readonly DbContext context;
        protected readonly DbSet<TEntity> dbSet;

        public Repository(DbContext context)
        {
            this.context = context;
            dbSet = context.Set<TEntity>();
        }

        public IQueryable<TEntity> GetAll()
        {
            return dbSet.AsNoTracking();
        }

        public TEntity GetById(int id)
        {
            return dbSet.FirstOrDefault(e => e.Id == id);
        }

        public async Task Create(TEntity entity)
        {
            await dbSet.AddAsync(entity);
        }

        public async Task Update(TEntity entity)
        {
            dbSet.Update(entity);
        }
    }
}
