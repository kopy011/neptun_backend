using Microsoft.EntityFrameworkCore;
using neptun_backend.Context;
using neptun_backend.Entities;
using neptun_backend.UnitOfWork;

namespace neptun_backend.Services
{
    public interface IStudentService
    {
        IEnumerable<Student> getAll();
        IEnumerable<Course> getAllCourse(string NeptunCode, int SemesterId);
    }

    public class StudentService : AbstractService, IStudentService
    {

        public StudentService(IUnitOfWork unitOfWork) : base(unitOfWork) 
        {
        }

        public IEnumerable<Student> getAll()
        {
            return unitOfWork.GetRepository<Student>().GetAll();
        }

        public IEnumerable<Course> getAllCourse(string NeptunCode, int SemesterId)
        {
            return unitOfWork.GetRepository<Student>().GetAll().Include(s => s.Courses.Where(c => c.Semester.Id == SemesterId)).Where(s => s.NeptunCode.Equals(NeptunCode)).FirstOrDefault()?.Courses ?? new List<Course>();
        }
    }
}
