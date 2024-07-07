using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Theater.Application.Repositories;

namespace Theater.Application.Modules.ActorModule.Queries.ActorGetByIdQuery
{
    public class ActorGetByIdRequestHandler : IRequestHandler<ActorGetByIdRequest, ActorRequestDto>
    {
        private readonly IActorRepository _actorRepository;
        private readonly IActionContextAccessor _ctx;

        public ActorGetByIdRequestHandler(IActorRepository actorRepository, IActionContextAccessor ctx)
        {
            _actorRepository = actorRepository;
            _ctx = ctx;
        }

        public async Task<ActorRequestDto> Handle(ActorGetByIdRequest request, CancellationToken cancellationToken)
        {
            var entity = await _actorRepository.GetAll()
                .Include(a => a.Roles)
                .ThenInclude(r => r.Poster)
                .Where(a => a.Id == request.Id && a.DeletedAt == null)
                .FirstOrDefaultAsync(cancellationToken);

            if (entity == null)
            {
                return null;
            }
            var filteredRoles = entity.Roles.Where(r => r.DeletedAt == null).ToList();

            string host = $"{_ctx.ActionContext.HttpContext.Request.Scheme}://{_ctx.ActionContext.HttpContext.Request.Host}";

            return new ActorRequestDto
            {
                Id = entity.Id,
                FullName = entity.FullName,
                Title = entity.Title,
                ImageSrc = $"{host}/uploads/images/{entity.ImageSrc}",
                Roles = filteredRoles.Select(r => new RoleRequestDto
                {
                    Id = r.Id,
                    RoleName = r.RoleName,
                    PosterId = r.PosterId,
                    PosterTitle = r.Poster.Title
                }).ToList()
            };
        }
    }
}
