namespace neptun_backend.Entities
{
    public class Person : AbstractEntity
    {
        public string NeptunCode { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<Course>? Courses { get; set; }
    }
}
