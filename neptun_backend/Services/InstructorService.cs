using Microsoft.AspNetCore.Identity;
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
    public interface IInstructorService : IPersonService<Instructor>
    {
        IEnumerable<InstructorStudentsDTO> GetAllStudents(int InstructorId, int SemesterId);
        public IEnumerable<StatPerSemesterDTO> GetSemesterStatistics(int InstructorId);
    }

    public class InstructorService : PersonService<Instructor>, IInstructorService
    {
        public InstructorService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager): base(unitOfWork, userManager)
        {
        }

        public IEnumerable<InstructorStudentsDTO> GetAllStudents(int InstructorId, int SemesterId)
        {
            var courseIds = from course in GetAllCourse(InstructorId, SemesterId, IgnoreFilters: true) select course.Id;
            var coursesWithStudents = unitOfWork.GetRepository<Course>().GetAll(ignoreFilters: true).Include(c => c.Students).Where(c => courseIds.Any(cId => cId == c.Id));
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
