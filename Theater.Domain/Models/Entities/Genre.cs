using Theater.Infrastructure.Abstracts;
using Theater.Infrastructure.Concrates;

namespace Theater.Domain.Models.Entities
{
    public class Genre : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
