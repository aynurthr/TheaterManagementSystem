using MediatR;
using System.Collections.Generic;

namespace Theater.Application.Modules.HallModule.Queries.HallGetAllQuery
{
    public class HallGetAllRequest : IRequest<IEnumerable<HallRequestDto>>
    {
        public bool OnlyAvailable { get; set; } = true;

    }
}
