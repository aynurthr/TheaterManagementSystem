using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Theater.Application.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

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

            string host = $"{_ctx.ActionContext.HttpContext.Request.Scheme}://{_ctx.ActionContext.HttpContext.Request.Host}";

            return new ActorRequestDto
            {
                Id = entity.Id,
                FullName = entity.FullName,
                Title = entity.Title,
                ImageSrc = $"{host}/uploads/images/{entity.ImageSrc}",
                Roles = entity.Roles.Select(r => new RoleRequestDto
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
