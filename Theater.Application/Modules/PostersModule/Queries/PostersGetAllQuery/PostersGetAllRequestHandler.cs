using Theater.Application.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace Theater.Application.Modules.PosterModule.Queries.PosterGetAllQuery
{
    public class PosterGetAllRequestHandler : IRequestHandler<PosterGetAllRequest, IEnumerable<PosterGetAllRequestDto>>
    {
        private readonly IPosterRepository posterRepository;
        private readonly IActionContextAccessor ctx;

        public PosterGetAllRequestHandler(IPosterRepository posterRepository, IActionContextAccessor ctx)
        {
            this.posterRepository = posterRepository;
            this.ctx = ctx;
        }

        public async Task<IEnumerable<PosterGetAllRequestDto>> Handle(PosterGetAllRequest request, CancellationToken cancellationToken)
        {
            var query = posterRepository.GetAll();

            if (request.OnlyAvailable)
            {
                query = query.Where(m => m.DeletedAt == null);
            }

            string host = $"{ctx.ActionContext.HttpContext.Request.Scheme}://{ctx.ActionContext.HttpContext.Request.Host}";

            var queryResponse = await query.Select(m => new PosterGetAllRequestDto
            {
                Id = m.Id,
                Title = m.Title,
                ImageUrl = $"{host}/uploads/images/{m.ImageSrc}",
                Description = m.Description,
                ShowDate = m.ShowDates
                             .Where(sd => sd.Date >= DateTime.Now)
                             .OrderBy(sd => sd.Date)
                             .FirstOrDefault().Date,
                Age = m.Age,
                Genre = m.Genre.Name
            }).ToListAsync(cancellationToken);

            return queryResponse;
        }
    }
}
