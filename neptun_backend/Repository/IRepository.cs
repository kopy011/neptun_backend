namespace neptun_backend.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll(bool tracking = false, bool ignoreFilters = false);

        Task<TEntity> GetById(int id, bool tracking = false);

        Task<TEntity> Create(TEntity entity);

        void Update(TEntity entity);
    }
}
