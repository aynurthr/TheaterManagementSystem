using Theater.Application.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Theater.Application.Modules.TeamMemberModule.Queries.TeamMemberGetAllQuery
{
    class TeamMemberGetAllRequestHandler : IRequestHandler<TeamMemberGetAllRequest, IEnumerable<TeamMemberRequestDto>>
    {
        private readonly ITeamMemberRepository teamMemberRepository;
        private readonly IActionContextAccessor ctx;

        public TeamMemberGetAllRequestHandler(ITeamMemberRepository teamMemberRepository, IActionContextAccessor ctx)
        {
            this.teamMemberRepository = teamMemberRepository;
            this.ctx = ctx;
        }

        public async Task<IEnumerable<TeamMemberRequestDto>> Handle(TeamMemberGetAllRequest request, CancellationToken cancellationToken)
        {
            var query = teamMemberRepository.GetAll();

            if (request.OnlyAvailable)
            {
                query = query.Where(m => m.DeletedAt == null);
            }

            string host = $"{ctx.ActionContext.HttpContext.Request.Scheme}://{ctx.ActionContext.HttpContext.Request.Host}";
            var queryResponse = await query
                .Select(m => new TeamMemberRequestDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    Role = m.Role,
                    Biography = m.Biography,
                    ImageUrl = $"{host}/uploads/images/{m.ImageUrl}"
                }).ToListAsync(cancellationToken);

            return queryResponse;
        }
    }
}
