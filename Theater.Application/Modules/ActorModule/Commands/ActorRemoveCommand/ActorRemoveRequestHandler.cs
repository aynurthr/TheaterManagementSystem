using MediatR;
using Theater.Application.Repositories;
using System.Threading;
using System.Threading.Tasks;
using Theater.Infrastructure.Exceptions;

namespace Theater.Application.Modules.ActorModule.Commands.ActorRemoveCommand
{
    public class ActorRemoveRequestHandler : IRequestHandler<ActorRemoveRequest>
    {
        private readonly IActorRepository _actorRepository;

        public ActorRemoveRequestHandler(IActorRepository actorRepository)
        {
            _actorRepository = actorRepository;
        }

        public async Task Handle(ActorRemoveRequest request, CancellationToken cancellationToken)
        {
            var entity = await _actorRepository.GetAsync(m => m.Id == request.Id && m.DeletedAt == null, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException("Actor entity not found.", request.Id);
            }

            _actorRepository.Remove(entity);
            await _actorRepository.SaveAsync(cancellationToken);
        }
    }
}

