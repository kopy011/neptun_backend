﻿using Microsoft.EntityFrameworkCore;
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

        public IQueryable<TEntity> GetAll(bool tracking = false, bool ignoreFilters = false)
        {
            if (ignoreFilters)
            {
                return tracking ? dbSet.IgnoreQueryFilters().AsQueryable() : dbSet.IgnoreQueryFilters().AsNoTracking();
            }
            else
            {
                return tracking ? dbSet.AsQueryable() : dbSet.AsNoTracking();
            }
        }

        public async Task<TEntity> GetById(int id, bool tracking = false)
        {
            return tracking ? await dbSet.FirstOrDefaultAsync(e => e.Id == id)
                : await dbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<TEntity> Create(TEntity entity)
        {
            return (await dbSet.AddAsync(entity)).Entity;
        }

        public void Update(TEntity entity)
        {
            dbSet.Update(entity);
        }
    }
}
