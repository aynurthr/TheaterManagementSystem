using MediatR;

namespace Theater.Application.Modules.PosterModule.Commands.PosterRemoveCommand
{
    public class PosterRemoveRequest : IRequest
    {
        public int Id { get; set; }
    }
}
