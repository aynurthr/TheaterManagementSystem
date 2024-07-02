using MediatR;
using Theater.Application.Modules.TeamMemberModule.Queries.TeamMemberGetAllQuery;

namespace Theater.Application.Modules.TeamMemberModule.Queries.TeamMemberGetByIdQuery
{
    public class TeamMemberGetByIdRequest : IRequest<TeamMemberRequestDto>
    {
        public int Id { get; set; }
    }
}
