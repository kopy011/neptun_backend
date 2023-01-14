using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace neptun_backend.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string Name { get; set; }
        public string NeptunCode { get; set; }
        public string Department { get; set; }
        public int? StudentId{ get; set; }
        public int? InstructorId { get; set; }
        public bool isDeleted { get; set; } = false;
    }
}
