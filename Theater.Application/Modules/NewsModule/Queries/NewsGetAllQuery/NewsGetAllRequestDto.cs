namespace Theater.Application.Modules.NewsModule.Queries.NewsGetAllQuery
{
    public class NewsGetAllRequestDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; } 
        public string ImageUrl { get; set; } 
        public DateTime PublishedAt { get; set; }
    }
}
