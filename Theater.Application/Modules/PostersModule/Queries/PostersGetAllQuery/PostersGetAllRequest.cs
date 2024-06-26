﻿using MediatR;

namespace Theater.Application.Modules.PosterModule.Queries.PosterGetAllQuery
{
    public class PosterGetAllRequest : IRequest<IEnumerable<PosterGetAllRequestDto>>
    {
        public bool OnlyAvailable { get; set; } = true;
    }
}
