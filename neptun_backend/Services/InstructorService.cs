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
        IEnumerable<Instructor> getAll();
        IEnumerable<Course> getAllCourse(int instructorId, int semesterId);
        int Create(Instructor instructorData);
        int Update(Instructor instructorData);
        int Delete(int id);
        int takeACourse(int instructorId, int courseId);
    }

    public class InstructorService : AbstractService, IInstructorService
    {
        public InstructorService(IUnitOfWork unitOfWork): base(unitOfWork)
        {
        }

        public IEnumerable<Instructor> getAll()
        {
            return unitOfWork.GetRepository<Instructor>().GetAll();
        }

        public IEnumerable<Course> getAllCourse(int instructorId, int semesterId)
        {
            return unitOfWork.GetRepository<Instructor>().GetAll().Include(i => i.Courses.Where(c => c.Semester.Id == semesterId)).Where(i => i.Id == instructorId).FirstOrDefault().Courses ?? new List<Course>();
        }

        public int Create(Instructor instructorData)
        {
            unitOfWork.GetRepository<Instructor>().Create(instructorData);
            return unitOfWork.SaveChanges();
        }

        public int Update(Instructor instructorData)
        {
            unitOfWork.GetRepository<Instructor>().Update(instructorData);
            return unitOfWork.SaveChanges();
        }

        public int Delete(int id)
        {
            Instructor instructor = unitOfWork.GetRepository<Instructor>().GetById(id);
            instructor.isDeleted = true;
            return unitOfWork.SaveChanges();
        }

        public int takeACourse(int instructorId, int courseId)
        {
            var instructor = unitOfWork.GetRepository<Instructor>().GetAll().Include(i => i.Courses).FirstOrDefault(i => i.Id == instructorId)
                ?? throw new Exception("Instructor not found!");
            var course = unitOfWork.GetRepository<Course>().GetAll().Include(c => c.Instructors).FirstOrDefault(c => c.Id == courseId)
                ?? throw new Exception("Course not found!");

            if (instructor.Courses.Contains(course))
            {
                throw new Exception("Instructor already instructs in the given course!");
            }

            instructor.Courses.Add(course);
            course.Instructors.Add(instructor);
            unitOfWork.GetRepository<Instructor>().Update(instructor);
            unitOfWork.GetRepository<Course>().Update(course);

            return unitOfWork.SaveChanges();
        }
    }
}
