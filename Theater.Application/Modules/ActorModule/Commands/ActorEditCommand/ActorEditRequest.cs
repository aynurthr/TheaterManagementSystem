using MediatR;
using Microsoft.AspNetCore.Http;
using Theater.Application.Modules.ActorModule.Queries;

namespace Theater.Application.Modules.ActorModule.Commands.ActorEditCommand
{
    public class ActorEditRequest : IRequest<ActorRequestDto>
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public IFormFile Image { get; set; }
        public string ImageUrl { get; set; }

    }
}
