using MediatR;
using Theater.Application.Repositories;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Threading;
using System.Threading.Tasks;
using Theater.Application.Modules.TeamMemberModule.Queries.TeamMemberGetAllQuery;

namespace Theater.Application.Modules.TeamMemberModule.Queries.TeamMemberGetByIdQuery
{
    public class TeamMemberGetByIdRequestHandler : IRequestHandler<TeamMemberGetByIdRequest, TeamMemberRequestDto>
    {
        private readonly ITeamMemberRepository _teamMemberRepository;
        private readonly IActionContextAccessor _ctx;

        public TeamMemberGetByIdRequestHandler(ITeamMemberRepository teamMemberRepository, IActionContextAccessor ctx)
        {
            _teamMemberRepository = teamMemberRepository;
            _ctx = ctx;
        }

        public async Task<TeamMemberRequestDto> Handle(TeamMemberGetByIdRequest request, CancellationToken cancellationToken)
        {
            var entity = await _teamMemberRepository.GetAsync(m => m.Id == request.Id && m.DeletedAt == null, cancellationToken);

            if (entity == null)
            {
                return null;
            }

            string host = $"{_ctx.ActionContext.HttpContext.Request.Scheme}://{_ctx.ActionContext.HttpContext.Request.Host}";
            var dto = new TeamMemberRequestDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Role = entity.Role,
                Biography = entity.Biography,
                ImageUrl = $"{host}/uploads/images/{entity.ImageUrl}"
            };

            return dto;
        }
    }
}
