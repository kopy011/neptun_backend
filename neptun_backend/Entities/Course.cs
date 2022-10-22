using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace neptun_backend.Entities
{
    public class Course : AbstractEntity
    {
        public string name { get; set; }
        public string code { get; set; }
        public int credit { get; set; }
        public string department { get; set; }
    }

}
