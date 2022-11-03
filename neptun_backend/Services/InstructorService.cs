using Microsoft.EntityFrameworkCore;
using neptun_backend.Context;
using neptun_backend.Entities;
using neptun_backend.Repository;
using neptun_backend.UnitOfWork;
using System.ComponentModel.DataAnnotations;
using System.Transactions;

namespace neptun_backend.Services
{
    public interface IInstructorService
    {
        IEnumerable<Instructor> GetAll();
        IEnumerable<Course> GetAllCourse(int instructorId, int semesterId);
        Task Create(Instructor instructorData);
        Task Update(Instructor instructorData);
        Task Delete(int id);
        Task TakeACourse(int instructorId, int courseId);
    }

    public class InstructorService : AbstractService, IInstructorService
    {
        public InstructorService(IUnitOfWork unitOfWork): base(unitOfWork)
        {
        }

        public IEnumerable<Instructor> GetAll()
        {
            return unitOfWork.GetRepository<Instructor>().GetAll();
        }

        public IEnumerable<Course> GetAllCourse(int instructorId, int semesterId)
        {
            return unitOfWork.GetRepository<Instructor>().GetAll().Include(i => i.Courses.Where(c => c.Semester.Id == semesterId)).Where(i => i.Id == instructorId).FirstOrDefault().Courses ?? new List<Course>();
        }

        public async Task Create(Instructor instructorData)
        {
            await unitOfWork.GetRepository<Instructor>().Create(instructorData);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task Update(Instructor instructorData)
        {
            unitOfWork.GetRepository<Instructor>().Update(instructorData);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            Instructor instructor = await unitOfWork.GetRepository<Instructor>().GetById(id);

            if(instructor == null)
            {
                throw new Exception("Instructor not found");
            }

            instructor.isDeleted = true;
            await unitOfWork.SaveChangesAsync();
        }

        public async Task TakeACourse(int instructorId, int courseId)
        {
            var instructor = unitOfWork.GetRepository<Instructor>().GetAll(tracking: true).Include(i => i.Courses).FirstOrDefault(i => i.Id == instructorId)
                ?? throw new Exception("Instructor not found!");
            var course = unitOfWork.GetRepository<Course>().GetAll(tracking: true).Include(c => c.Instructors).FirstOrDefault(c => c.Id == courseId)
                ?? throw new Exception("Course not found!");

            if (instructor.Courses.Contains(course))
            {
                throw new Exception("Instructor already instructs in the given course!");
            }

            course.Instructors.Add(instructor);

            await unitOfWork.SaveChangesAsync();
        }
    }
}
