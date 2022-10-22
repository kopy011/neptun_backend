using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace neptun_backend.Entities
{
    public class Semester : AbstractEntity
    {
        public string name { get; set; }

        public DateTime startDate { get; set; }

        public DateTime endDate { get; set; }
    }

}
