using MediatR;
using Theater.Application.Repositories;
using Theater.Domain.Models.Entities;
using Theater.Infrastructure.Abstracts;
using System.Threading;
using System.Threading.Tasks;
using Theater.Application.Modules.TeamMemberModule.Queries;

namespace Theater.Application.Modules.TeamMemberModule.Commands.TeamMemberAddCommand
{
    public class TeamMemberAddRequestHandler : IRequestHandler<TeamMemberAddRequest, TeamMemberRequestDto>
    {
        private readonly ITeamMemberRepository _teamMemberRepository;
        private readonly IFileService _fileService;

        public TeamMemberAddRequestHandler(ITeamMemberRepository teamMemberRepository, IFileService fileService)
        {
            _teamMemberRepository = teamMemberRepository;
            _fileService = fileService;
        }

        public async Task<TeamMemberRequestDto> Handle(TeamMemberAddRequest request, CancellationToken cancellationToken)
        {
            var entity = new TeamMember
            {
                Name = request.Name,
                Role = request.Role,
                Biography = request.Biography,
            };


            if (request.Image != null)
            {
                entity.ImageUrl = await _fileService.UploadAsync(request.Image);
            }

            await _teamMemberRepository.AddAsync(entity, cancellationToken);
            await _teamMemberRepository.SaveAsync(cancellationToken);

            return new TeamMemberRequestDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Role = entity.Role,
                Biography = entity.Biography,
                ImageUrl = entity.ImageUrl
            };
        }
    }
}
