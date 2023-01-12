using Microsoft.EntityFrameworkCore;
using neptun_backend.Entities;
using neptun_backend.Repository;

namespace neptun_backend.UnitOfWork
{
    public class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        private readonly TContext _context;
        private bool _disposed;
        private Dictionary<Type, object> _repositories;

        public UnitOfWork(TContext context)
        {
            _context = context;
        }

        public DbContext Context()
        {
            return _context;
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if(_repositories != null)
                    {
                        _repositories.Clear();
                    }

                    _context.Dispose();
                }
            }

            _disposed = true;
        }

        public DbSet<TEntity> getDbSet<TEntity>() where TEntity : AbstractEntity
        {
            throw new NotImplementedException();
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : AbstractEntity
        {
            if(_repositories == null)
            {
                _repositories = new Dictionary<Type, object>();
            }

            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new Repository<TEntity>(_context);
            }

            return (IRepository<TEntity>)_repositories[type];
        }

        public int SaveChanges()
        {
            try
            {
                int count = _context.SaveChanges();
                return count;
            } catch
            {
                throw;
            }
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
