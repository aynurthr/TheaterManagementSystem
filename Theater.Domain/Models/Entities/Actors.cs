using Theater.Infrastructure.Abstracts;
using Theater.Infrastructure.Concrates;

namespace Theater.Domain.Models.Entities
{
    public class Actor : AuditableEntity
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public string ImageSrc { get; set; }

        public ICollection<Role> Roles { get; set; }
    }
}
