using MediatR;
using Theater.Application.Repositories;
using Theater.Domain.Models.Entities;
using Theater.Infrastructure.Abstracts;
using System.Threading;
using System.Threading.Tasks;
using Theater.Application.Modules.TeamMemberModule.Queries.TeamMemberGetAllQuery;

namespace Theater.Application.Modules.TeamMemberModule.Commands.TeamMemberEditCommand
{
    public class TeamMemberEditRequestHandler : IRequestHandler<TeamMemberEditRequest, TeamMember>
    {
        private readonly ITeamMemberRepository _teamMemberRepository;
        private readonly IFileService _fileService;

        public TeamMemberEditRequestHandler(ITeamMemberRepository teamMemberRepository, IFileService fileService)
        {
            _teamMemberRepository = teamMemberRepository;
            _fileService = fileService;
        }

        public async Task<TeamMember> Handle(TeamMemberEditRequest request, CancellationToken cancellationToken)
        {
            var entity = await _teamMemberRepository.GetAsync(m => m.Id == request.Id && m.DeletedAt == null, cancellationToken);

            if (entity == null)
            {
                return null;
            }

            entity.Name = request.Name;
            entity.Role = request.Role;
            entity.Biography = request.Biography;

            if (request.Image != null)
            {
                entity.ImageUrl = await _fileService.ChangeFileAsync(entity.ImageUrl, request.Image);
            }

            _teamMemberRepository.Edit(entity);
            await _teamMemberRepository.SaveAsync(cancellationToken);

            return entity;
        }
    }
}
