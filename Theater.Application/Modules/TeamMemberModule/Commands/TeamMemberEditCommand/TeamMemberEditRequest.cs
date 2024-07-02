using MediatR;
using Microsoft.AspNetCore.Http;
using Theater.Domain.Models.Entities;

namespace Theater.Application.Modules.TeamMemberModule.Commands.TeamMemberEditCommand
{
    public class TeamMemberEditRequest : IRequest<TeamMember>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Biography { get; set; }
        public IFormFile Image { get; set; }
    }
}
