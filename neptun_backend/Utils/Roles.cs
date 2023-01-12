using neptun_backend.Entities;

namespace neptun_backend.Utils
{
    public static class Roles
    {
        public static readonly string ADMIN = "Admin";
        public static readonly string STUDENT = "Student";
        public static readonly string INSTRUCTOR = "Instructor";
        public static bool isValidRole(string role)
        {
            return role == ADMIN || role == STUDENT || role == INSTRUCTOR;
        }
    }
}
