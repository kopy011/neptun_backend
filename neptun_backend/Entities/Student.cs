using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace neptun_backend.Entities
{
    public class Student : AbstractEntity
    {
        public enum MajorType
        {
            mérnökinformatikusMsc,
            programtervezőinformatikusMsc,
            mérnökinformatikusBsc,
            programtervezőinformatikusBsc,
            gazdaságinformatikusBsc
        }

        public string NeptunCode { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public MajorType Major { get; set; }

        public List<Course> Courses { get; set; }
    }
}
