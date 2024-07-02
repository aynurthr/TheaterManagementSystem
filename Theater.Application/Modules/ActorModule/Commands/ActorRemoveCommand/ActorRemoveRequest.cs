using MediatR;

namespace Theater.Application.Modules.ActorModule.Commands.ActorRemoveCommand
{
    public class ActorRemoveRequest : IRequest
    {
        public int Id { get; set; }
    }
}
