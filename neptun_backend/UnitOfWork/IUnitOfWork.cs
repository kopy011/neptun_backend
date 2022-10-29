using Microsoft.EntityFrameworkCore;
using neptun_backend.Entities;
using neptun_backend.Repository;

namespace neptun_backend.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : AbstractEntity;

        DbSet<TEntity> getDbSet<TEntity>() where TEntity : AbstractEntity;

        int SaveChanges();

        Task<int> SaveChangesAsync();

        DbContext Context();
    }
}
