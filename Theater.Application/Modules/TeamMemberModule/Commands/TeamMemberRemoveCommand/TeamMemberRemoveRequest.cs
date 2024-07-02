using MediatR;

namespace Theater.Application.Modules.TeamMemberModule.Commands.TeamMemberRemoveCommand
{
    public class TeamMemberRemoveRequest : IRequest
    {
        public int Id { get; set; }
    }
}
