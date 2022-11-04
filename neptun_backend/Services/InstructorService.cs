using Microsoft.EntityFrameworkCore;
using neptun_backend.Context;
using neptun_backend.DTOS;
using neptun_backend.Entities;
using neptun_backend.Repository;
using neptun_backend.UnitOfWork;
using System.ComponentModel.DataAnnotations;
using System.Transactions;

namespace neptun_backend.Services
{
    public interface IInstructorService : IAbstractService<Instructor>
    {
        IEnumerable<Course> GetAllCourse(int InstructorId, int SemesterId, bool IgnoreFilters = false);
        Task TakeACourse(int InstructorId, int CourseId);
        IEnumerable<InstructorStudentsDTO> GetAllStudents(int InstructorId, int SemesterId);
        public IEnumerable<StatPerSemesterDTO> GetSemesterStatistics(int InstructorId);
    }

    public class InstructorService : AbstractService<Instructor>, IInstructorService
    {
        public InstructorService(IUnitOfWork unitOfWork): base(unitOfWork)
        {
        }

        public IEnumerable<Course> GetAllCourse(int InstructorId, int SemesterId, bool IgnoreFilters = false)
        {
            return unitOfWork.GetRepository<Instructor>().GetAll(ignoreFilters: IgnoreFilters)
                .Include(i => i.Courses.Where(c => c.Semester.Id == SemesterId))
                .Where(i => i.Id == InstructorId).FirstOrDefault()?.Courses
                ?? new List<Course>();
        }

        public IEnumerable<InstructorStudentsDTO> GetAllStudents(int InstructorId, int SemesterId)
        {
            var courseIds = from course in GetAllCourse(InstructorId, SemesterId, IgnoreFilters: true) select course.Id;
            var coursesWithStudents = unitOfWork.GetRepository<Course>().GetAll(ignoreFilters: true).Include(c => c.Students).Where(c => courseIds.Any(cId => cId == c.Id));
            Console.WriteLine(coursesWithStudents.Count());
            List<InstructorStudentsDTO> students = new List<InstructorStudentsDTO>();
            
            foreach(var course in coursesWithStudents)
            {
                foreach(var student in course.Students)
                {
                    students.Add(new InstructorStudentsDTO
                    {
                        Name = student.Name,
                        NeptunCode = student.NeptunCode,
                        CourseCode = course.Code
                    });
                }
            }

            return students.OrderBy(s => s.CourseCode).ThenBy(s => s.Name);
        }

        public async Task TakeACourse(int InstructorId, int CourseId)
        {
            var instructor = unitOfWork.GetRepository<Instructor>().GetAll(tracking: true).Include(i => i.Courses).FirstOrDefault(i => i.Id == InstructorId)
                ?? throw new Exception("Instructor not found!");
            var course = unitOfWork.GetRepository<Course>().GetAll(tracking: true).Include(c => c.Instructors).FirstOrDefault(c => c.Id == CourseId)
                ?? throw new Exception("Course not found!");

            if (instructor.Courses.Contains(course))
            {
                throw new Exception("Instructor already instructs in the given course!");
            }

            course.Instructors.Add(instructor);

            await unitOfWork.SaveChangesAsync();
        }

        public IEnumerable<StatPerSemesterDTO> GetSemesterStatistics(int InstructorId)
        {
            var semesterIds = from semester in unitOfWork.GetRepository<Semester>().GetAll(ignoreFilters: true) select semester.Id;
            List<StatPerSemesterDTO> SemesterStatistics = new List<StatPerSemesterDTO>();
            
            foreach(var sId in semesterIds)
            {
                var courses = GetAllCourse(InstructorId, sId, IgnoreFilters: true);
                var students = GetAllStudents(InstructorId, sId);
                List<string> countedStudentNeptunCodes = new List<string>();

                int credits = courses.Sum(c => c.Credit);
                int studentCount = 0;
                foreach(var student in students)
                {
                    if (!countedStudentNeptunCodes.Contains(student.NeptunCode))
                    {
                        countedStudentNeptunCodes.Add(student.NeptunCode);
                        studentCount++;
                    }
                }

                SemesterStatistics.Add(new StatPerSemesterDTO
                {
                    SemesterId = sId,
                    Credits = credits,
                    Students = studentCount
                });
            }

            return SemesterStatistics;
        }
    }
}
