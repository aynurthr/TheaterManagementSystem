using Theater.Infrastructure.Abstracts;
using Theater.Infrastructure.Concrates;

namespace Theater.Domain.Models.Entities
{
    public class ShowDate : AuditableEntity
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int PosterId { get; set; }
        public Poster Poster { get; set; }

        public ICollection<SeatReservation> SeatReservations { get; set; }

    }
}
