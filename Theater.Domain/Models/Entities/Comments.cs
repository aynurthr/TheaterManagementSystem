using Theater.Domain.Models.Entities.Membership;
using Theater.Infrastructure.Abstracts;
using Theater.Infrastructure.Concrates;

namespace Theater.Domain.Models.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string CommentText { get; set; }
        public DateTime Time { get; set; }
        public int PosterId { get; set; }
        public Poster Poster { get; set; }
        public AppUser User { get; set; }

    }
}
