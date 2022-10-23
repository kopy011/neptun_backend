using Microsoft.EntityFrameworkCore;
using neptun_backend.Context;
using neptun_backend.Entities;

namespace neptun_backend.Services
{
    public interface IStudentService
    {
        List<Student> getAll();
        List<Course> getAllCourse(string NeptunCode, int SemesterId);
    }

    public class StudentService : IStudentService
    {
        private readonly NeptunBackendDbContext _dbContext;

        public StudentService(NeptunBackendDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Student> getAll()
        {
            return _dbContext.Students.ToList();
        }

        public List<Course> getAllCourse(string NeptunCode, int SemesterId)
        {
            return _dbContext.Students.Include(s => s.Courses.Where(c => c.Semester.Id == SemesterId)).Where(s => s.NeptunCode.Equals(NeptunCode)).FirstOrDefault()?.Courses ?? new List<Course>();
        }
    }
}
