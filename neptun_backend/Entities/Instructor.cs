using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace neptun_backend.Entities
{
    public class Instructor : AbstractEntity
    {
        public enum ClassificationType
        {
            docens,
            adjunktus,
            mesteroktató,
            ügyvívőSzakértő,
            tanársegéd,
            egyéb
        }

        public string NeptunCode { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public ClassificationType Classification { get; set; }
    }
}
