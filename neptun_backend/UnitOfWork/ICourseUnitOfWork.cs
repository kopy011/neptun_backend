using neptun_backend.Entities;

namespace neptun_backend.UnitOfWork
{
    public interface ICourseUnitOfWork
    {
        IEnumerable<Course> GetCoursesByDates(DateTime startDate, DateTime endDate, bool ignoreFilters = false);
    }
}
