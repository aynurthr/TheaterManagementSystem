using MediatR;

namespace Theater.Application.Modules.RoleModule.Commands.RoleEditCommand
{
    public class RoleEditRequest : IRequest<bool>
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public int ActorId { get; set; }
        public int PosterId { get; set; }
    }
}
