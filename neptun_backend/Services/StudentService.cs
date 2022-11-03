using Microsoft.EntityFrameworkCore;
using neptun_backend.Context;
using neptun_backend.Entities;
using neptun_backend.UnitOfWork;

namespace neptun_backend.Services
{
    public interface IStudentService
    {
        IEnumerable<Student> getAll();
        IEnumerable<Course> getAllCourse(int studentId, int semesterId);
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

        public IEnumerable<Course> getAllCourse(int studentId, int semesterId)
        {
            return unitOfWork.GetRepository<Student>().GetAll().Include(s => s.Courses.Where(c => c.Semester.Id == semesterId)).Where(s => s.Id == studentId).FirstOrDefault()?.Courses ?? new List<Course>();
        }
    }
}
