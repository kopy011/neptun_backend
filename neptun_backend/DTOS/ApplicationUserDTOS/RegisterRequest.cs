using neptun_backend.Utils;

namespace neptun_backend.DTOS.ApplicationUserDTOS
{
    public class RegisterRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Name { get; set; }
        public string NeptunCode { get; set; }
        public string? Department { get; set; }
        public string Role { get; set; } = Roles.STUDENT;
    }
}
