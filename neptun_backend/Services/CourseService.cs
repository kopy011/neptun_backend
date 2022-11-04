using Microsoft.EntityFrameworkCore;
using neptun_backend.Context;
using neptun_backend.Entities;
using neptun_backend.UnitOfWork;

namespace neptun_backend.Services
{
    public interface ICourseService : IAbstractService<Course>
    {
    }

    public class CourseService : AbstractService<Course>, ICourseService
    {

        public CourseService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
