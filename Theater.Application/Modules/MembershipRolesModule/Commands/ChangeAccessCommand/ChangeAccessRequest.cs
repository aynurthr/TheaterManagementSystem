using MediatR;

namespace Theater.Application.Modules.RolesModule.Commands.ChangeAccessCommand
{
    public class ChangeAccessRequest : IRequest
    {
        public string PolicyName { get; set; }
        public int RoleId { get; set; }
        public bool Selected { get; set; }
    }
}
