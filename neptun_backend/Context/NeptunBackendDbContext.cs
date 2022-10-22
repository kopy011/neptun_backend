using Microsoft.EntityFrameworkCore;
using neptun_backend.Entities;
using System.Reflection;

namespace neptun_backend.Context
{
    public class NeptunBackendDbContext : DbContext 
    {
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<Student> Students { get; set; }

        public NeptunBackendDbContext(DbContextOptions<NeptunBackendDbContext> options) : base(options)
        {
            Database.SetCommandTimeout(60);
        }
    }
}
