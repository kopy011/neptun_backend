namespace neptun_backend.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll(bool tracking = false);

        Task<TEntity> GetById(int id);

        Task Create(TEntity entity);

        void Update(TEntity entity);
    }
}
