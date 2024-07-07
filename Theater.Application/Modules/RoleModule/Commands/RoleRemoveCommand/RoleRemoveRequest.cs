using MediatR;

namespace Theater.Application.Modules.RoleModule.Commands.RoleRemoveCommand
{
    public class RoleRemoveRequest : IRequest<bool>
    {
        public int Id { get; set; }

        public RoleRemoveRequest(int id)
        {
            Id = id;
        }
    }
}
