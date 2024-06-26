using Theater.Infrastructure.Abstracts;
using Theater.Infrastructure.Concrates;

namespace Theater.Domain.Models.Entities
{
    public class SeatReservation : AuditableEntity
    {
        public int Id { get; set; }
        public int SeatId { get; set; }
        public Seat Seat { get; set; }
        public int ShowDateId { get; set; }
        public ShowDate ShowDate { get; set; }
        public string Email{ get; set; }
    }
}
