using Microsoft.EntityFrameworkCore;
using neptun_backend.Context;
using neptun_backend.Entities;
using neptun_backend.Repository;
using neptun_backend.UnitOfWork;
using System.ComponentModel.DataAnnotations;
using System.Transactions;

namespace neptun_backend.Services
{
    public interface IInstructorService : IAbstractService<Instructor>
    {
        IEnumerable<Course> GetAllCourse(int InstructorId, int SemesterId, bool IgnoreFilters = false);
        Task TakeACourse(int InstructorId, int CourseId);
    }

    public class InstructorService : AbstractService<Instructor>, IInstructorService
    {
        public InstructorService(IUnitOfWork unitOfWork): base(unitOfWork)
        {
        }

        public IEnumerable<Course> GetAllCourse(int InstructorId, int SemesterId, bool IgnoreFilters = false)
        {
            return unitOfWork.GetRepository<Instructor>().GetAll(ignoreFilters: IgnoreFilters)
                .Include(i => i.Courses.Where(c => c.Semester.Id == SemesterId))
                .Where(i => i.Id == InstructorId).FirstOrDefault()?.Courses
                ?? new List<Course>();
        }

        public async Task TakeACourse(int InstructorId, int CourseId)
        {
            var instructor = unitOfWork.GetRepository<Instructor>().GetAll(tracking: true).Include(i => i.Courses).FirstOrDefault(i => i.Id == InstructorId)
                ?? throw new Exception("Instructor not found!");
            var course = unitOfWork.GetRepository<Course>().GetAll(tracking: true).Include(c => c.Instructors).FirstOrDefault(c => c.Id == CourseId)
                ?? throw new Exception("Course not found!");

            if (instructor.Courses.Contains(course))
            {
                throw new Exception("Instructor already instructs in the given course!");
            }

            course.Instructors.Add(instructor);

            await unitOfWork.SaveChangesAsync();
        }
    }
}
