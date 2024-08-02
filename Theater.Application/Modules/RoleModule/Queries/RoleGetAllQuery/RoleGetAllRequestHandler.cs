using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Theater.Application.Repositories;
using Theater.Domain.Models.Entities;

namespace Theater.Application.Modules.RoleModule.Queries.RoleGetAllQuery
{
    public class RoleGetAllRequestHandler : IRequestHandler<RoleGetAllRequest, List<RoleRequestDto>>
    {
        private readonly IRoleRepository _roleRepository;

        public RoleGetAllRequestHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<List<RoleRequestDto>> Handle(RoleGetAllRequest request, CancellationToken cancellationToken)
        {
            var roles = await _roleRepository
                .GetAll()
                .Where(r => r.DeletedAt == null)
                .Include(r => r.Actor)
                .Include(r => r.Poster)
                .ToListAsync(cancellationToken);

            return roles.Select(r => new RoleRequestDto
            {
                Id = r.Id,
                RoleName = r.RoleName,
                ActorId = r.ActorId,
                ActorName = r.Actor.FullName,
                PosterId = r.PosterId,
                PosterTitle = r.Poster.Title
            }).ToList();
        }
    }
}
