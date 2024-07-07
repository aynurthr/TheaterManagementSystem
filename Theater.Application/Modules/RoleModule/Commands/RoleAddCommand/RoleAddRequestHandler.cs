using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Theater.Application.Repositories;
using Theater.Domain.Models.Entities;

namespace Theater.Application.Modules.RoleModule.Commands.RoleAddCommand
{
    public class RoleAddRequestHandler : IRequestHandler<RoleAddRequest, bool>
    {
        private readonly IRoleRepository _roleRepository;

        public RoleAddRequestHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<bool> Handle(RoleAddRequest request, CancellationToken cancellationToken)
        {
            var newRole = new Role
            {
                RoleName = request.RoleName,
                ActorId = request.ActorId,
                PosterId = request.PosterId
            };

            await _roleRepository.AddAsync(newRole, cancellationToken);
            await _roleRepository.SaveAsync(cancellationToken);

            return true;
        }
    }
}
