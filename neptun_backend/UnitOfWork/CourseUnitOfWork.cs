using Microsoft.EntityFrameworkCore;
using neptun_backend.Context;
using neptun_backend.Entities;

namespace neptun_backend.UnitOfWork
{
    public class CourseUnitOfWork<TContext> : UnitOfWork<TContext>, ICourseUnitOfWork where TContext : DbContext
    {
        public CourseUnitOfWork(TContext context) : base(context)
        {

        }

        public IEnumerable<Course> GetCoursesByDates(DateTime startDate, DateTime endDate, bool ignoreFilters = false)
        {
            return GetRepository<Course>().GetAll(ignoreFilters: ignoreFilters).Include(c => c.Semester)
                   .Where(c => (c.Semester.StartDate >= startDate && c.Semester.StartDate <= endDate)
                            || (c.Semester.EndDate >= startDate && c.Semester.EndDate <= endDate));
        }
    }
}
