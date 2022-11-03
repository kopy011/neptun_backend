namespace neptun_backend.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();

        TEntity GetById(int id);

        Task Create(TEntity entity);

        Task Update(TEntity entity);
    }
}
