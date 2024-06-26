using Theater.Infrastructure.Concrates;

namespace Theater.Domain.Models.Entities
{
    public class News : AuditableEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string ImageSrc { get; set; }
    }
}
