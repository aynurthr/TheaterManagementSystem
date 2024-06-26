
namespace Theater.Application.Modules.NewsModule.Queries.NewsGetByIdQuery
{
    public class NewsGetByIdRequestDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public DateTime PublishedAt { get; set; }

    }
}

