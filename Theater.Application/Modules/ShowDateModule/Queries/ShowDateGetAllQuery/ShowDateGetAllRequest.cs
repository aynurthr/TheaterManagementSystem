using MediatR;
using Theater.Application.Modules.ShowDateModule.Queries;

namespace Theater.Application.Modules.TeamMemberModule.Queries.TeamMemberGetAllQuery
{
    public class ShowDateGetAllRequest : IRequest<IEnumerable<ShowDateDto>>
    {
        public bool OnlyAvailable { get; set; } = true;
    }
}
