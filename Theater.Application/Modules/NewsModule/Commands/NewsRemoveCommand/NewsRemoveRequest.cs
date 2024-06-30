using MediatR;

namespace Theater.Application.Modules.NewsModule.Commands.NewsRemoveCommand
{
    public class NewsRemoveRequest : IRequest
    {
        public int Id { get; set; }
    }
}
