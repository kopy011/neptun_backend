using neptun_backend.Context;
using neptun_backend.Entities;

namespace neptun_backend.Services
{
    public interface IInstructorService
    {
        List<Instructor> getAll();
    }

    public class InstructorService : IInstructorService
    {
        private readonly NeptunBackendDbContext _dbContext;

        public InstructorService(NeptunBackendDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Instructor> getAll()
        {
            return _dbContext.Instructors.ToList();
        }
    }
}
