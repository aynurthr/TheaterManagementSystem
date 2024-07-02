using MediatR;
using Microsoft.AspNetCore.Http;
using Theater.Application.Modules.TeamMemberModule.Queries;

namespace Theater.Application.Modules.TeamMemberModule.Commands.TeamMemberAddCommand
{
    public class TeamMemberAddRequest : IRequest<TeamMemberRequestDto>
    {
        public string Name { get; set; }
        public string Role { get; set; }
        public string Biography { get; set; }
        public IFormFile Image { get; set; }
    }
}
