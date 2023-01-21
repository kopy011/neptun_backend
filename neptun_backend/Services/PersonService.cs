using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using neptun_backend.Entities;
using neptun_backend.UnitOfWork;
using neptun_backend.Utils;
using System.Security.Claims;
using System.Transactions;

namespace neptun_backend.Services
{
    public interface IPersonService<TEntity> : IAbstractService<TEntity> where TEntity : Person
    {
        IEnumerable<Course> GetAllCourse(int PersonId, int SemesterId, bool IgnoreFilters = false);
        Task TakeACourse(int PersonId, int CourseId);
    }

    public class PersonService<TEntity> : AbstractService<TEntity>, IPersonService<TEntity> where TEntity : Person
    {
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly ICourseService _courseService;
        protected readonly ICacheService _cacheService;
        public PersonService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, ICacheService cacheService, ICourseService courseService) : base(unitOfWork)
        {
            _userManager = userManager;
            _cacheService = cacheService;
            _courseService = courseService;
        }

        public IEnumerable<Course> GetAllCourse(int PersonId, int SemesterId, bool IgnoreFilters = false)
        {
            return _unitOfWork.GetRepository<TEntity>().GetAll(ignoreFilters: IgnoreFilters)
          .Include(i => i.Courses.Where(c => c.Semester.Id == SemesterId))
          .FirstOrDefault(i => i.Id == PersonId)?.Courses
          ?? new List<Course>();
        }

        public async Task TakeACourse(int PersonId, int CourseId)
        {
            var person = await _unitOfWork.GetRepository<TEntity>().GetAll(tracking: true).Include(i => i.Courses).FirstOrDefaultAsync(i => i.Id == PersonId)
                ?? throw new Exception("Person not found!");
            //TODO memory cache (talán az a megoldás hogy a tracking állítható legyen)
            var course = await _unitOfWork.GetRepository<Course>().GetAll(tracking: true).FirstOrDefaultAsync(c => c.Id == CourseId)
                ?? throw new Exception("Course not found!");

            if (person.Courses.Contains(course))
            {
                throw new Exception(person.GetType().ToString().Split('.').Last() + " already has the course!");
            }
            person.Courses.Add(course);
            await _unitOfWork.SaveChangesAsync();
        }

        public override async Task Delete(int PersonId)
        {
            var person = await _unitOfWork.GetRepository<TEntity>().GetById(PersonId);

            if(person == null)
            {
                throw new Exception("Person not found!");
            }

            //delete the person's user too (if there is any)
            var user = _userManager.Users.FirstOrDefault(u => u.NeptunCode == person.NeptunCode);

            if(user != null)
            {
                user.isDeleted = true;
                await _userManager.UpdateAsync(user);
            }

            await base.Delete(PersonId);
        }
    }
}
