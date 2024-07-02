using MediatR;
using System.Collections.Generic;

namespace Theater.Application.Modules.ActorModule.Queries.ActorGetAllQuery
{
    public class ActorGetAllRequest : IRequest<IEnumerable<ActorRequestDto>>
    {
        public bool OnlyAvailable { get; set; }
    }
}
