using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace neptun_backend.Entities
{
    public class Instructor : Person
    {
        //TODO: kiszervezni másik ősosztályba
        public enum ClassificationType
        {
            docens,
            adjunktus,
            mesteroktató,
            ügyvívőSzakértő,
            tanársegéd,
            egyéb
        }
        public ClassificationType Classification { get; set; }
        public List<Course> Courses { get; set; }
    }
}
