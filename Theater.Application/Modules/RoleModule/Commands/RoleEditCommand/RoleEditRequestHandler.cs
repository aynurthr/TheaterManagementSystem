using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Theater.Application.Repositories;
using Theater.Domain.Models.Entities;

namespace Theater.Application.Modules.RoleModule.Commands.RoleEditCommand
{
    public class RoleEditRequestHandler : IRequestHandler<RoleEditRequest, bool>
    {
        private readonly IRoleRepository _roleRepository;

        public RoleEditRequestHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<bool> Handle(RoleEditRequest request, CancellationToken cancellationToken)
        {
            var existingRole = await _roleRepository.GetAsync(r => r.Id == request.Id);
            if (existingRole == null)
            {
                return false;
            }

            existingRole.RoleName = request.RoleName;
            existingRole.ActorId = request.ActorId;
            existingRole.PosterId = request.PosterId;

            _roleRepository.Edit(existingRole);
            await _roleRepository.SaveAsync(cancellationToken);

            return true;
        }
    }
}
