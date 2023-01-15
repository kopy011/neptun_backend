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
        public PersonService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager) : base(unitOfWork)
        {
            _userManager = userManager;
        }

        public IEnumerable<Course> GetAllCourse(int PersonId, int SemesterId, bool IgnoreFilters = false)
        {
            return unitOfWork.GetRepository<TEntity>().GetAll(ignoreFilters: IgnoreFilters)
          .Include(i => i.Courses.Where(c => c.Semester.Id == SemesterId))
          .FirstOrDefault(i => i.Id == PersonId)?.Courses
          ?? new List<Course>();
        }

        public async Task TakeACourse(int PersonId, int CourseId)
        {
            var person = unitOfWork.GetRepository<TEntity>().GetAll(tracking: true).Include(i => i.Courses).FirstOrDefault(i => i.Id == PersonId)
                ?? throw new Exception("Person not found!");
            var course = unitOfWork.GetRepository<Course>().GetAll(tracking: true).Include(c => c.Instructors).FirstOrDefault(c => c.Id == CourseId)
                ?? throw new Exception("Course not found!");

            if (person.Courses.Contains(course))
            {
                throw new Exception(person.GetType().ToString().Split('.').Last() + " already has the course!");
            }

            person.Courses.Add(course);

            await unitOfWork.SaveChangesAsync();
        }

        public override async Task Delete(int PersonId)
        {
            var person = await unitOfWork.GetRepository<TEntity>().GetById(PersonId);

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
