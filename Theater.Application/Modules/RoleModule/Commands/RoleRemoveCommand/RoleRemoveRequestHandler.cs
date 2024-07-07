using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Theater.Application.Repositories;
using Theater.Domain.Models.Entities;

namespace Theater.Application.Modules.RoleModule.Commands.RoleRemoveCommand
{
    public class RoleRemoveRequestHandler : IRequestHandler<RoleRemoveRequest, bool>
    {
        private readonly IRoleRepository _roleRepository;

        public RoleRemoveRequestHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<bool> Handle(RoleRemoveRequest request, CancellationToken cancellationToken)
        {
            var existingRole = await _roleRepository.GetAsync(r => r.Id == request.Id);
            if (existingRole == null)
            {
                return false;
            }

            _roleRepository.Remove(existingRole);
            await _roleRepository.SaveAsync(cancellationToken);

            return true;
        }
    }
}
