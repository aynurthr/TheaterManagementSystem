using Theater.Infrastructure.Abstracts;
using System.Collections.Generic;
using Theater.Infrastructure.Concrates;

namespace Theater.Domain.Models.Entities
{
    public class Seat : AuditableEntity
    {
        public int Id { get; set; }
        public int Row { get; set; }
        public int Number { get; set; }
        public decimal Price { get; set; }
        public int HallId { get; set; }
        public Hall Hall { get; set; }

        public ICollection<SeatReservation> SeatReservations { get; set; }
    }
}
