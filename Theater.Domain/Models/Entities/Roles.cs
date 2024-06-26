using Theater.Infrastructure.Abstracts;
using Theater.Infrastructure.Concrates;

namespace Theater.Domain.Models.Entities
{
    public class Role : AuditableEntity
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public int ActorId { get; set; }
        public Actor Actor { get; set; }
        public int PosterId { get; set; }
        public Poster Poster { get; set; }
    }
}
