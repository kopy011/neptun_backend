using Microsoft.EntityFrameworkCore;
using neptun_backend.Context;
using neptun_backend.Entities;
using neptun_backend.UnitOfWork;

namespace neptun_backend.Services
{
    public interface ICourseService : IAbstractService<Course>
    {
        public IEnumerable<Course> getCoursesByDates(DateTime startDate, DateTime endDate, bool ignoreFilters = false);
        public IEnumerable<Course> getHardCourses(bool ignoreFilters = false);
        public IEnumerable<Course> getCoursesByNeptunCode(List<int> courseIds, string neptunCode, bool ignoreFilters = false);
    }

    public class CourseService : AbstractService<Course>, ICourseService
    {
        private readonly ICourseUnitOfWork courseUnitOfWork;

        public CourseService(IUnitOfWork unitOfWork, ICourseUnitOfWork _courseUnitOfWork) : base(unitOfWork)
        {
            courseUnitOfWork = _courseUnitOfWork;
        }

        public override async Task Update(Course course)
        {
            if(course.ScheduleInformation == null)
            {
                var savedCourse = await unitOfWork.GetRepository<Course>().GetAll().Where(c => c.Id == course.Id).FirstOrDefaultAsync();
                course.ScheduleInformation = savedCourse?.ScheduleInformation;
            }

            unitOfWork.GetRepository<Course>().Update(course);
            await unitOfWork.SaveChangesAsync();
        }

        public IEnumerable<Course> getCoursesByDates(DateTime startDate, DateTime endDate, bool ignoreFilters = false)
        {
            return courseUnitOfWork.GetCoursesByDates(startDate, endDate, ignoreFilters);
        }

        public IEnumerable<Course> getHardCourses(bool ignoreFilters = false)
        {
            return unitOfWork.GetRepository<Course>().GetAll(ignoreFilters).Include(c => c.Instructors).Where(c => c.Credit >= 4);
        }

        public IEnumerable<Course> getCoursesByNeptunCode(List<int> courseIds, string neptunCode, bool ignoreFilters = false)
        {
            return unitOfWork.GetRepository<Course>().GetAll(ignoreFilters).Include(c => c.Students)
                .Where(c => courseIds.Contains(c.Id) && c.Students.Any(s => s.NeptunCode == neptunCode));
        }
    }
}
