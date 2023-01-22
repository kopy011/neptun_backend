using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using neptun_backend.Entities;
using neptun_backend.UnitOfWork;

namespace neptun_backend.Services
{
    public interface IAbstractService<TEntity> where TEntity : AbstractEntity
    {
        IEnumerable<TEntity> GetAll(bool IgnoreFilters = false);
        Task<TEntity> GetById(int EntityId, bool tracking = false);
        Task<TEntity> Create(TEntity Entity);
        Task Update(TEntity Entity);
        Task Delete(int EntityId);
    }

    public class AbstractService<TEntity> : IAbstractService<TEntity> where TEntity : AbstractEntity
    {
        protected readonly IUnitOfWork _unitOfWork;

        public AbstractService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public virtual IEnumerable<TEntity> GetAll(bool IgnoreFilters = false)
        {
            return _unitOfWork.GetRepository<TEntity>().GetAll(ignoreFilters: IgnoreFilters);
        }

        public virtual async Task<TEntity> GetById(int EntityId, bool tracking = false)
        {
            return await _unitOfWork.GetRepository<TEntity>().GetById(EntityId, tracking);
        }

        public virtual async Task<TEntity> Create(TEntity Entity)
        {
            var createdEntity = await _unitOfWork.GetRepository<TEntity>().Create(Entity);
            await _unitOfWork.SaveChangesAsync();
            return createdEntity;
        }

        public virtual async Task Update(TEntity Entity)
        {
            _unitOfWork.GetRepository<TEntity>().Update(Entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public virtual async Task Delete(int EntityId)
        {
            TEntity entity = await _unitOfWork.GetRepository<TEntity>().GetById(EntityId);

            if (entity == null)
            {
                throw new Exception(typeof(TEntity).Name + " not found");
            }

            entity.isDeleted = true;
            await _unitOfWork.SaveChangesAsync();
        }


    }
}
