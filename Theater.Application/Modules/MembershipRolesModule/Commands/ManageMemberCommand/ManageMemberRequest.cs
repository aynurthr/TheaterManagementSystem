using MediatR;

namespace Theater.Application.Modules.RolesModule.Commands.ManageMemberCommand
{
    public class ManageMemberRequest : IRequest
    {
        //int memberId, int roleId, bool selected
        public int MemberId { get; set; }
        public int RoleId { get; set; }
        public bool Selected { get; set; }
    }
}
