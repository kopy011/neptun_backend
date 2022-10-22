using neptun_backend.Context;
using neptun_backend.Entities;

namespace neptun_backend.Services
{
    public interface ISemesterService
    {
        List<Semester> getAll();
    }

    public class SemesterService : ISemesterService
    {
        private readonly NeptunBackendDbContext _dbContext;

        public SemesterService(NeptunBackendDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Semester> getAll()
        {
            return _dbContext.Semesters.ToList();
        }
    }
}
