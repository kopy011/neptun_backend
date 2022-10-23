using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace neptun_backend.Entities
{
    public class Course : AbstractEntity
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public int Credit { get; set; }

        public string Department { get; set; }

        [ForeignKey("SemesterId")]
        public Semester? Semester { get; set; }

        public List<Instructor> Instructors { get; set; }

        public List<Student> Students { get; set; }
    }

}
