using Microsoft.EntityFrameworkCore;
using neptun_backend.Context;
using neptun_backend.Entities;
using neptun_backend.UnitOfWork;

namespace neptun_backend.Services
{
    public interface IStudentService : IAbstractService<Student>
    {
        IEnumerable<Course> GetAllCourse(int StudentId, int SemesterId);
        Task TakeACourse(int StudentId, int CourseId);
    }

    public class StudentService : AbstractService<Student>, IStudentService
    {

        public StudentService(IUnitOfWork unitOfWork) : base(unitOfWork) 
        {
        }

        public IEnumerable<Course> GetAllCourse(int StudentId, int SemesterId)
        {
            return unitOfWork.GetRepository<Student>().GetAll().Include(s => s.Courses.Where(c => c.Semester.Id == SemesterId)).Where(s => s.Id == StudentId).FirstOrDefault()?.Courses ?? new List<Course>();
        }

        public async Task TakeACourse(int StudentId, int CourseId)
        {
            var student = unitOfWork.GetRepository<Student>().GetAll(tracking: true).Include(i => i.Courses).FirstOrDefault(i => i.Id == StudentId)
                ?? throw new Exception("Student not found!");
            var course = unitOfWork.GetRepository<Course>().GetAll(tracking: true).Include(c => c.Students).FirstOrDefault(c => c.Id == CourseId)
                ?? throw new Exception("Course not found!");

            if (student.Courses.Contains(course))
            {
                throw new Exception("Student already attends the given course!");
            }

            course.Students.Add(student);

            await unitOfWork.SaveChangesAsync();
        }
    }
}
