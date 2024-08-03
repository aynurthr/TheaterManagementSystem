namespace Theater.Domain.Models.Entities
{
    public class Newsletter
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public bool IsSubscribed { get; set; }
        public DateTime SubscribedAt { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
