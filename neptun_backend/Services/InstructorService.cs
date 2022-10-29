using Microsoft.EntityFrameworkCore;
using neptun_backend.Context;
using neptun_backend.Entities;
using neptun_backend.UnitOfWork;

namespace neptun_backend.Services
{
    public interface IInstructorService
    {
        IEnumerable<Instructor> getAll();
        IEnumerable<Course> getAllCourse(string NeptunCode, int SemesterId);
    }

    public class InstructorService : AbstractService, IInstructorService
    {
        public InstructorService(IUnitOfWork unitOfWork): base(unitOfWork)
        {
        }

        public IEnumerable<Instructor> getAll()
        {
            return unitOfWork.GetRepository<Instructor>().GetAll();
        }
        public IEnumerable<Course> getAllCourse(string NeptunCode, int SemesterId)
        {
            return unitOfWork.GetRepository<Instructor>().GetAll().Include(i => i.Courses.Where(c => c.Semester.Id == SemesterId)).Where(i => i.NeptunCode.Equals(NeptunCode)).FirstOrDefault()?.Courses ?? new List<Course>();
        }
    }
}
