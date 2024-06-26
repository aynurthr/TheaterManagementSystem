using Theater.Infrastructure.Abstracts;
using Theater.Infrastructure.Concrates;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theater.Domain.Models.Entities
{
    public class Hall : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Rows { get; set; }

        // This property is not mapped by EF Core
        [NotMapped]
        public List<int> SeatsPerRow { get; set; }

        // This property stores the JSON string for SeatsPerRow
        public string SeatsPerRowJson { get; set; }

        public ICollection<Poster> Posters { get; set; }
        public ICollection<Seat> Seats { get; set; }
    }
}
