using MediatR;
using Theater.Application.Repositories;
using Theater.Domain.Models.Entities;
using System.Threading;
using System.Threading.Tasks;
using Theater.Infrastructure.Exceptions;

namespace Theater.Application.Modules.TeamMemberModule.Commands.TeamMemberRemoveCommand
{
    public class TeamMemberRemoveRequestHandler : IRequestHandler<TeamMemberRemoveRequest>
    {
        private readonly ITeamMemberRepository _teamMemberRepository;

        public TeamMemberRemoveRequestHandler(ITeamMemberRepository teamMemberRepository)
        {
            _teamMemberRepository = teamMemberRepository;
        }

        public async Task Handle(TeamMemberRemoveRequest request, CancellationToken cancellationToken)
        {
            var entity = await _teamMemberRepository.GetAsync(m => m.Id == request.Id && m.DeletedAt == null, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException("TeamMember entity not found.", request.Id);
            }

            _teamMemberRepository.Remove(entity);
            await _teamMemberRepository.SaveAsync(cancellationToken);

        }
    }
}
