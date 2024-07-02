using MediatR;
using Microsoft.AspNetCore.Http;
using Theater.Application.Modules.ActorModule.Queries;

namespace Theater.Application.Modules.ActorModule.Commands.ActorAddCommand
{
    public class ActorAddRequest : IRequest<ActorRequestDto>
    {
        public string FullName { get; set; }
        public string Title { get; set; }
        public IFormFile Image { get; set; }
    }
}
