using Theater.Infrastructure.Abstracts;
using Theater.Infrastructure.Concrates;

namespace Theater.Domain.Models.Entities
{
    public class Poster : AuditableEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
        public string Age { get; set; }
        public string Duration { get; set; }
        public string Description { get; set; }
        public string ImageSrc { get; set; }
   
        public double Rating { get; set; }

        public ICollection<Role> Roles { get; set; }
        public ICollection<ShowDate> ShowDates { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
