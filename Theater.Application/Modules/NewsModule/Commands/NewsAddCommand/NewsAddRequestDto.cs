using Microsoft.AspNetCore.Http;

namespace Theater.Application.Modules.NewsModule.Commands.NewsAddCommand
{
    public class NewsAddRequestDto
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
}
