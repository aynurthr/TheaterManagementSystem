using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Theater.Application.Repositories;
using Theater.Domain.Models.Entities;

namespace Theater.Application.Modules.RoleModule.Queries.RoleGetByIdQuery
{
    public class RoleGetByIdRequestHandler : IRequestHandler<RoleGetByIdRequest, RoleRequestDto>
    {
        private readonly IRoleRepository _roleRepository;

        public RoleGetByIdRequestHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<RoleRequestDto> Handle(RoleGetByIdRequest request, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.GetAll()
                                            .Include(r => r.Actor)
                                            .Include(r => r.Poster)
                                            .FirstOrDefaultAsync(r => r.Id == request.Id && r.DeletedAt == null, cancellationToken);

            if (role == null)
            {
                return null;
            }

            return new RoleRequestDto
            {
                Id = role.Id,
                RoleName = role.RoleName,
                ActorId = role.ActorId,
                ActorName = role.Actor.FullName,
                PosterId = role.PosterId,
                PosterTitle = role.Poster.Title
            };
        }
    }
}
