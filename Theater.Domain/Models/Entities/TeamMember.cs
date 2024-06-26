using Theater.Infrastructure.Concrates;

namespace Theater.Domain.Models.Entities
{
    public class TeamMember : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Biography { get; set; }
        public string ImageUrl { get; set; }
    }
}

