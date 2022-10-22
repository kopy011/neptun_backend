using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace neptun_backend.Entities
{
    public class Course : AbstractEntity
    {
        public string name { get; set; }

        public string code { get; set; }

        public int credit { get; set; }

        public string department { get; set; }

        public List<Instructor> instructors { get; set; }

        [ForeignKey("SemesterId")]
        public Semester? semester { get; set; }

        public List<Student> students { get; set; }
    }

}
