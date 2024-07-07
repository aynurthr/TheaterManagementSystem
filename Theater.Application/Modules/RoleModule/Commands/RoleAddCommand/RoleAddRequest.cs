using MediatR;

namespace Theater.Application.Modules.RoleModule.Commands.RoleAddCommand
{
    public class RoleAddRequest : IRequest<bool>
    {
        public string RoleName { get; set; }
        public int ActorId { get; set; }
        public int PosterId { get; set; }
    }
}
