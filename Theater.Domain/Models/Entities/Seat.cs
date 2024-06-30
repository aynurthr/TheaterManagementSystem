using Theater.Infrastructure.Abstracts;
using Theater.Infrastructure.Concrates;

namespace Theater.Domain.Models.Entities
{
    public class Seat : AuditableEntity
    {
        public int Id { get; set; }
        public int HallId { get; set; }
        public string Row { get; set; }
        public int Number { get; set; }
        public decimal Price { get; set; }

        public Hall Hall { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    }
}
