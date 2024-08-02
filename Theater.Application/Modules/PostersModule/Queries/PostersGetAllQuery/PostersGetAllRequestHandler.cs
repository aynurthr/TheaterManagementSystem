using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Theater.Application.Modules.PosterModule.Queries.PosterGetAllQuery;
using Theater.Application.Repositories;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

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
        var query = posterRepository.GetAll().Where(m => m.DeletedAt == null);

        if (request.GenreId.HasValue)
        {
            query = query.Where(m => m.GenreId == request.GenreId.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.SearchQuery))
        {
            query = query.Where(m => m.Title.Contains(request.SearchQuery));
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
