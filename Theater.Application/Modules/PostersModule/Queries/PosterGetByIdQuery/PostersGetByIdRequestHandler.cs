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
    private readonly IPosterRepository posterRepository;
    private readonly IActionContextAccessor ctx;

    public PosterGetByIdRequestHandler(IPosterRepository posterRepository, IActionContextAccessor ctx)
    {
        this.posterRepository = posterRepository;
        this.ctx = ctx;
    }

    public async Task<PosterGetByIdRequestDto> Handle(PosterGetByIdRequest request, CancellationToken cancellationToken)
    {
        var poster = await posterRepository.GetAll()
            .Include(p => p.Roles).ThenInclude(r => r.Actor)
            .Include(p => p.Comments).ThenInclude(c => c.User) // Include the User
            .Include(p => p.Genre) // Include Genre
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (poster == null)
        {
            return null;
        }

        string host = $"{ctx.ActionContext.HttpContext.Request.Scheme}://{ctx.ActionContext.HttpContext.Request.Host}";

        return new PosterGetByIdRequestDto
        {
            Id = poster.Id,
            Title = poster.Title,
            Genre = poster.Genre.Name, // Get Genre name
            Duration = poster.Duration,
            Age = poster.Age,
            Description = poster.Description,
            ImageUrl = $"{host}/uploads/images/{poster.ImageSrc}",
            Rating = poster.Rating,
            Actors = poster.Roles.Select(r => new ActorDto
            {
                FullName = r.Actor.FullName,
                Role = r.RoleName,
                Title = r.Actor.Title,
                ImageSrc = $"{host}/uploads/actors/{r.Actor.ImageSrc}"
            }).ToList(),
            Comments = poster.Comments.Select(c => new CommentDto
            {
                Id = c.Id,
                UserName = c.User.UserName, // Get the UserName from the User
                Text = c.CommentText,
                CreatedAt = c.Time
            }).ToList()
        };
    }
}
