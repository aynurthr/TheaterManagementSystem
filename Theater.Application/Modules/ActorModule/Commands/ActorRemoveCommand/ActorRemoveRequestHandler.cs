using MediatR;
using Theater.Application.Repositories;
using System.Threading;
using System.Threading.Tasks;
using Theater.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Theater.Application.Modules.ActorModule.Commands.ActorRemoveCommand
{
    public class ActorRemoveRequestHandler : IRequestHandler<ActorRemoveRequest, string>
    {
        private readonly IActorRepository _actorRepository;
        private readonly IRoleRepository _roleRepository;

        public ActorRemoveRequestHandler(IActorRepository actorRepository, IRoleRepository roleRepository)
        {
            _actorRepository = actorRepository;
            _roleRepository = roleRepository;
        }

        public async Task<string> Handle(ActorRemoveRequest request, CancellationToken cancellationToken)
        {
            var entity = await _actorRepository.GetAsync(m => m.Id == request.Id && m.DeletedAt == null, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException("Actor entity not found.", request.Id);
            }

            var rolesUsingActor = await _roleRepository.GetAll(r => r.ActorId == request.Id && r.DeletedAt == null)
                .Include(r => r.Poster)
                .Where(r => r.Poster.DeletedAt == null)
                .ToListAsync(cancellationToken);

            if (rolesUsingActor.Any())
            {
                var count = rolesUsingActor.Count;
                return $"This actor is used by {count} role(s). Change their actor to be able to delete this actor.";
            }

            _actorRepository.Remove(entity);
            await _actorRepository.SaveAsync(cancellationToken);
            return "success";
        }
    }
}
