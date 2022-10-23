using Microsoft.EntityFrameworkCore;
using neptun_backend.Context;
using neptun_backend.Entities;

namespace neptun_backend.Services
{
    public interface IInstructorService
    {
        List<Instructor> getAll();
        List<Course> getAllCourse(string NeptunCode);
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
        public List<Course> getAllCourse(string NeptunCode)
        {
            Instructor? instructor = _dbContext.Instructors.Include(i => i.Courses).FirstOrDefault(i => i.NeptunCode == NeptunCode);
            return instructor?.Courses ?? new List<Course>();
        }
    }
}
