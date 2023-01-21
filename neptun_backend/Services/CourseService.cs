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
        private readonly ICourseUnitOfWork _courseUnitOfWork;
        private readonly ICacheService _cacheService;

        public CourseService(IUnitOfWork unitOfWork, ICourseUnitOfWork courseUnitOfWork, ICacheService cacheService) : base(unitOfWork)
        {
            _courseUnitOfWork = courseUnitOfWork;
            _cacheService = cacheService;
        }

        public override IEnumerable<Course> GetAll(bool IgnoreFilters = false)
        {
            _cacheService.GetAll<Course>();

            return base.GetAll(IgnoreFilters);
        }

        public override async Task Update(Course course)
        {
            if(course.ScheduleInformation == null)
            {
                var savedCourse = await _unitOfWork.GetRepository<Course>().GetAll().Where(c => c.Id == course.Id).FirstOrDefaultAsync();
                course.ScheduleInformation = savedCourse?.ScheduleInformation;
            }

            _unitOfWork.GetRepository<Course>().Update(course);
            await _unitOfWork.SaveChangesAsync();
        }

        public IEnumerable<Course> getCoursesByDates(DateTime startDate, DateTime endDate, bool ignoreFilters = false)
        {
            return _courseUnitOfWork.GetCoursesByDates(startDate, endDate, ignoreFilters);
        }

        public IEnumerable<Course> getHardCourses(bool ignoreFilters = false)
        {
            return _unitOfWork.GetRepository<Course>().GetAll(ignoreFilters).Include(c => c.Instructors).Where(c => c.Credit >= 4);
        }

        public IEnumerable<Course> getCoursesByNeptunCode(List<int> courseIds, string neptunCode, bool ignoreFilters = false)
        {
            return _unitOfWork.GetRepository<Course>().GetAll(ignoreFilters).Include(c => c.Students)
                .Where(c => courseIds.Contains(c.Id) && c.Students.Any(s => s.NeptunCode == neptunCode));
        }
    }
}
