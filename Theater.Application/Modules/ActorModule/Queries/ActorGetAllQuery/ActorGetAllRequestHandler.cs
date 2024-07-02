using MediatR;
using Theater.Application.Repositories;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Theater.Application.Modules.ActorModule.Queries.ActorGetAllQuery
{
    public class ActorGetAllRequestHandler : IRequestHandler<ActorGetAllRequest, IEnumerable<ActorRequestDto>>
    {
        private readonly IActorRepository _actorRepository;
        private readonly IActionContextAccessor _ctx;

        public ActorGetAllRequestHandler(IActorRepository actorRepository, IActionContextAccessor ctx)
        {
            _actorRepository = actorRepository;
            _ctx = ctx;
        }

        public async Task<IEnumerable<ActorRequestDto>> Handle(ActorGetAllRequest request, CancellationToken cancellationToken)
        {
            var query = _actorRepository.GetAll();

            if (request.OnlyAvailable)
            {
                query = query.Where(m => m.DeletedAt == null);
            }


            string host = $"{_ctx.ActionContext.HttpContext.Request.Scheme}://{_ctx.ActionContext.HttpContext.Request.Host}";
            var queryResponse = await query
                .OrderBy(m => m.FullName)
                .Select(m => new ActorRequestDto
                {
                    Id = m.Id,
                    FullName = m.FullName,
                    Title = m.Title,
                    ImageSrc = $"{host}/uploads/images/{m.ImageSrc}"
                }).ToListAsync(cancellationToken);

            return queryResponse;
        }
    }
}
