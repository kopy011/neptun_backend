using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace neptun_backend.Entities
{
    public class Student : AbstractEntity
    {
        public enum Major
        {
            mérnökinformatikusMsc,
            programtervezőinformatikusMsc,
            mérnökinformatikusBsc,
            programtervezőinformatikusBsc,
            gazdaságinformatikusBsc
        }

        public string neptunCode { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string major { get; set; }
    }
}
