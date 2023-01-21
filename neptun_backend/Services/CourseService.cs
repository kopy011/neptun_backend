using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
            return _cacheService.GetAll<Course>();
        }

        public override async Task<Course> GetById(int CourseId)
        {
            Course course;
            if(!_cacheService.TryGetValue(CourseId, out course))
            {
                course = await base.GetById(CourseId);
            }
            if(course == null)
            {
                throw new Exception("Course not found!");
            }
            return course;
        }

        public override async Task<Course> Create(Course course)
        {
            var createdCourse = await base.Create(course);
            createdCourse = await _unitOfWork.Context().Set<Course>().Include(c => c.Instructors).Include(c => c.Students)
                .Where(c => c.Id == createdCourse.Id).FirstOrDefaultAsync();
            _cacheService.Set(createdCourse.Id, createdCourse);
            return createdCourse;
        }

        public override async Task Update(Course course)
        {
            if(course.ScheduleInformation == null)
            {
                var savedCourse = await _unitOfWork.GetRepository<Course>().GetAll().Where(c => c.Id == course.Id).FirstOrDefaultAsync();
                course.ScheduleInformation = savedCourse?.ScheduleInformation;
            }

            await base.Update(course);
            course = await _unitOfWork.Context().Set<Course>().Include(c => c.Instructors).Include(c => c.Instructors)
                .Where(c => c.Id == course.Id).FirstOrDefaultAsync();
            _cacheService.Set(course.Id, course);
        }

        public async override Task Delete(int EntityId)
        {
            await base.Delete(EntityId);
            _cacheService.Remove(EntityId);
        }

        public IEnumerable<Course> getCoursesByDates(DateTime startDate, DateTime endDate, bool ignoreFilters = false)
        {
            //TODO cache általakítás
            return _courseUnitOfWork.GetCoursesByDates(startDate, endDate, ignoreFilters);
        }

        public IEnumerable<Course> getHardCourses(bool ignoreFilters = false)
        {
            return _cacheService.GetAll<Course>().Where(c => c.Credit >= 4);
        }

        public IEnumerable<Course> getCoursesByNeptunCode(List<int> courseIds, string neptunCode, bool ignoreFilters = false)
        {
            return _cacheService.GetAll<Course>().Where(c => courseIds.Contains(c.Id) && c.Students.Any(s => s.NeptunCode == neptunCode));
        }
    }
}
