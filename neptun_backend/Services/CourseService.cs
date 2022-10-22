using neptun_backend.Context;
using neptun_backend.Entities;

namespace neptun_backend.Services
{
    public interface ICourseService
    {
        List<Course> getAll();
    }

    public class CourseService : ICourseService
    {
        private readonly NeptunBackendDbContext _dbContext;

        public CourseService(NeptunBackendDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Course> getAll()
        {
            return _dbContext.Courses.ToList();
        }
    }
}
