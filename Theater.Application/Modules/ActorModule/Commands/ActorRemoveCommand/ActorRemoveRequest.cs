using MediatR;

namespace Theater.Application.Modules.ActorModule.Commands.ActorRemoveCommand
{
    public class ActorRemoveRequest : IRequest<string>
    {
        public int Id { get; set; }
    }
}
