using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using neptun_backend.Context;
using neptun_backend.Entities;
using neptun_backend.UnitOfWork;

namespace neptun_backend.Services
{
    public interface IStudentService : IPersonService<Student>
    {
    }

    public class StudentService : PersonService<Student>, IStudentService
    {
        public StudentService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, ICacheService cacheService, ICourseService courseService) : 
            base(unitOfWork, userManager, cacheService, courseService) 
        {
        }
    }
}
