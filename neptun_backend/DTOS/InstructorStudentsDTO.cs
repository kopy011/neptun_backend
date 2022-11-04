using System.ComponentModel.DataAnnotations;

namespace neptun_backend.DTOS
{
    public class InstructorStudentsDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string NeptunCode { get; set; }

        [Required]
        public string CourseCode { get; set; }
    }
}
