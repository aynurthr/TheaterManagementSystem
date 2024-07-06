using Theater.Infrastructure.Abstracts;
using Theater.Infrastructure.Concrates;

namespace Theater.Domain.Models.Entities
{
    public class Ticket : AuditableEntity
    {
        public int Id { get; set; }
        public int ShowDateId { get; set; }
        public int SeatId { get; set; }
        public decimal Price { get; set; }
        public bool IsPurchased { get; set; }
        public int? IsPurchasedBy { get; set; }
        public DateTime? IsPurchasedAt { get; set; }
        public ShowDate ShowDate { get; set; }
        public Seat Seat { get; set; }
    }
}
