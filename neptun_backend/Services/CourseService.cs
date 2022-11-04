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

        public override async Task Update(Course course)
        {
            if(course.ScheduleInformation == null)
            {
                var savedCourse = await unitOfWork.GetRepository<Course>().GetAll().Where(c => c.Id == course.Id).FirstOrDefaultAsync();
                course.ScheduleInformation = savedCourse.ScheduleInformation;
            }

            unitOfWork.GetRepository<Course>().Update(course);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
