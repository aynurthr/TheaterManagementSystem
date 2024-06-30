using MediatR;
using Microsoft.AspNetCore.Http;

namespace Theater.Application.Modules.NewsModule.Commands.NewsAddCommand
{
    public class NewsAddRequest : IRequest<NewsAddRequestDto>
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
    }
}
