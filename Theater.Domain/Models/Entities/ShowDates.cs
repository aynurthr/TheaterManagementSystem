using System.Net.Sockets;
using Theater.Infrastructure.Abstracts;
using Theater.Infrastructure.Concrates;

namespace Theater.Domain.Models.Entities
{
    public class ShowDate : AuditableEntity
    {
        public int Id { get; set; }
        public int PosterId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public int HallId { get; set; }

        public Poster Poster { get; set; }
        public Hall Hall { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    }
}
