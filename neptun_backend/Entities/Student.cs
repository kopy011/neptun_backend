using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace neptun_backend.Entities
{
    public class Student : Person
    {
        public enum MajorType
        {
            mérnökinformatikusMsc,
            programtervezőinformatikusMsc,
            mérnökinformatikusBsc,
            programtervezőinformatikusBsc,
            gazdaságinformatikusBsc
        }
        public MajorType Major { get; set; }
    }
}
