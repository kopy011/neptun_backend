using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace neptun_backend.Entities
{
    public class Instructor : AbstractEntity
    {
        public enum Classification
        {
            docens,
            adjunktus,
            mesteroktató,
            ügyvívőSzakértő,
            tanársegéd,
            egyéb
        }

        public string neptunCode { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string classificitioan { get; set; }
    }
}
