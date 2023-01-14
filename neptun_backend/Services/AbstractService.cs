using Microsoft.EntityFrameworkCore;
using neptun_backend.Entities;
using neptun_backend.UnitOfWork;

namespace neptun_backend.Services
{
    public interface IAbstractService<TEntity> where TEntity : AbstractEntity
    {
        IEnumerable<TEntity> GetAll(bool IgnoreFilters = false);
        Task Create(TEntity Entity);
        Task Update(TEntity Entity);
        Task Delete(int EntityId);
    }

    public class AbstractService<TEntity> : IAbstractService<TEntity> where TEntity : AbstractEntity
    {
        protected IUnitOfWork unitOfWork;

        public AbstractService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<TEntity> GetAll(bool IgnoreFilters = false)
        {
            return unitOfWork.GetRepository<TEntity>().GetAll(ignoreFilters: IgnoreFilters);
        }

        public async Task Create(TEntity Entity)
        {
            await unitOfWork.GetRepository<TEntity>().Create(Entity);
            await unitOfWork.SaveChangesAsync();
        }

        public virtual async Task Update(TEntity Entity)
        {
            unitOfWork.GetRepository<TEntity>().Update(Entity);
            await unitOfWork.SaveChangesAsync();
        }

        public virtual async Task Delete(int EntityId)
        {
            TEntity entity = await unitOfWork.GetRepository<TEntity>().GetById(EntityId);

            if (entity == null)
            {
                throw new Exception(typeof(TEntity).Name + " not found");
            }

            entity.isDeleted = true;
            await unitOfWork.SaveChangesAsync();
        }


    }
}
