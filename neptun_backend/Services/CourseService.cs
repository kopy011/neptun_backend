using Microsoft.EntityFrameworkCore;
using neptun_backend.Context;
using neptun_backend.Entities;
using neptun_backend.UnitOfWork;

namespace neptun_backend.Services
{
    public interface ICourseService
    {
        IEnumerable<Course> getAll();
    }

    public class CourseService : AbstractService, ICourseService
    {

        public CourseService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public IEnumerable<Course> getAll()
        {
            return unitOfWork.GetRepository<Course>().GetAll().Include(c => c.Semester);
        }
    }
}
