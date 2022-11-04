using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace neptun_backend.Entities
{
    public class Course : AbstractEntity
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public int Credit { get; set; }

        public string Department { get; set; }

        public int SemesterId { get; set; }
        public Semester? Semester { get; set; }

        public List<Instructor> Instructors { get; set; }

        public List<Student> Students { get; set; }

        public string? ScheduleInformation { get; set; }
    }

}
