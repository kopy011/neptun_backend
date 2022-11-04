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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().HasQueryFilter(c => !c.isDeleted);
            modelBuilder.Entity<Instructor>().HasQueryFilter(i => !i.isDeleted);
            modelBuilder.Entity<Semester>().HasQueryFilter(s => !s.isDeleted);
            modelBuilder.Entity<Student>().HasQueryFilter(s => !s.isDeleted);

            modelBuilder.Entity<Course>().Property(c => c.ScheduleInformation).HasDefaultValue("ismeretlen");
        }
    }
}
