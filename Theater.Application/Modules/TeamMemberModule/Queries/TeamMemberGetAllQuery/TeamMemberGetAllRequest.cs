using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theater.Application.Modules.NewsModule.Queries.NewsGetAllQuery;

namespace Theater.Application.Modules.TeamMemberModule.Queries.TeamMemberGetAllQuery
{
    public class TeamMemberGetAllRequest : IRequest<IEnumerable<TeamMemberGetAllRequestDto>>
    {
        public bool OnlyAvailable { get; set; } = true;
    }
}
