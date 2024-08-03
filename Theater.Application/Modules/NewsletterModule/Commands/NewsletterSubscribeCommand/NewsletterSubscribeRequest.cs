
using MediatR;

namespace Theater.Application.Modules.NewsletterModule.Commands.NewsletterSubscribeCommand
{
    public class NewsletterSubscribeRequest : IRequest
    {
        public string Email { get; set; }
    }
}
