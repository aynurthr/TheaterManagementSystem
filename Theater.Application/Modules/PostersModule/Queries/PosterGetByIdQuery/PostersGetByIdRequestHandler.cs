using Theater.Application.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Theater.Application.Modules.PosterModule.Queries.PosterGetByIdQuery;

public class PosterGetByIdRequestHandler : IRequestHandler<PosterGetByIdRequest, PosterGetByIdRequestDto>
{
    private readonly IPosterRepository _posterRepository;
    private readonly IActionContextAccessor _ctx;

    public PosterGetByIdRequestHandler(IPosterRepository posterRepository, IActionContextAccessor ctx)
    {
        _posterRepository = posterRepository;
        _ctx = ctx;
    }

    public async Task<PosterGetByIdRequestDto> Handle(PosterGetByIdRequest request, CancellationToken cancellationToken)
    {
        var poster = await _posterRepository.GetAll()
            .Where(p => p.DeletedAt == null)
            .Include(p => p.Roles).ThenInclude(r => r.Actor)
            .Include(p => p.Comments).ThenInclude(c => c.User)
            .Include(p => p.Genre)
            .Include(p => p.ShowDates)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (poster == null)
        {
            return null;
        }

        // Filter out roles where DeletedAt is not null
        var filteredRoles = poster.Roles.Where(r => r.DeletedAt == null).ToList();

        string host = $"{_ctx.ActionContext.HttpContext.Request.Scheme}://{_ctx.ActionContext.HttpContext.Request.Host}";

        return new PosterGetByIdRequestDto
        {
            Id = poster.Id,
            Title = poster.Title,
            Genre = poster.Genre.Name,
            GenreId=poster.GenreId,
            Duration = poster.Duration,
            Age = poster.Age,
            Description = poster.Description,
            ImageUrl = $"{host}/uploads/images/{poster.ImageSrc}",
            Rating = poster.Rating,
            Actors = filteredRoles.Select(r => new ActorDto
            {
                Id = r.Actor.Id,
                FullName = r.Actor.FullName,
                Role = r.RoleName,
                Title = r.Actor.Title,
                ImageSrc = $"{host}/uploads/images/{r.Actor.ImageSrc}"
            }).ToList(),
            Comments = poster.Comments.Select(c => new CommentDto
            {
                Id = c.Id,
                UserName = c.User.UserName,
                Text = c.CommentText,
                CreatedAt = c.Time
            }).ToList(),
            ShowDates = poster.ShowDates.Select(sd => new ShowDateDto // Add this
            {
                ShowDateId = sd.Id,
                Date = sd.Date,
                HallId = sd.HallId
            }).ToList()
        };
    }
}
