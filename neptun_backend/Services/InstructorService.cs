using neptun_backend.Context;
using neptun_backend.Entities;

namespace neptun_backend.Services
{
    public interface IInstructorService
    {
        List<Instructor> getAll();
        //List<Course> getAllCourse();
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

        //TODO: letisztázni az oktatók és tárgyak közti kapcsolatot

        //public List<Course> getAllCourse()
        //{
        //    return _dbContext.Courses.Where(c => c.)
        //}
    }
}
