using MediatR;
using System.Collections.Generic;

namespace Theater.Application.Modules.GenreModule.Queries.GenreGetAllQuery
{
    public class GenreGetAllRequest : IRequest<IEnumerable<GenreRequestDto>>
    {
        public bool OnlyAvailable { get; set; } = true;

    }
}
