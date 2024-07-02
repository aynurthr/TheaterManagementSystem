using MediatR;
using Theater.Application.Repositories;
using Theater.Domain.Models.Entities;
using Theater.Infrastructure.Abstracts;
using System.Threading;
using System.Threading.Tasks;
using Theater.Application.Modules.ActorModule.Queries;

namespace Theater.Application.Modules.ActorModule.Commands.ActorAddCommand
{
    public class ActorAddRequestHandler : IRequestHandler<ActorAddRequest, ActorRequestDto>
    {
        private readonly IActorRepository _actorRepository;
        private readonly IFileService _fileService;

        public ActorAddRequestHandler(IActorRepository actorRepository, IFileService fileService)
        {
            _actorRepository = actorRepository;
            _fileService = fileService;
        }

        public async Task<ActorRequestDto> Handle(ActorAddRequest request, CancellationToken cancellationToken)
        {
            var imageSrc = await _fileService.UploadAsync(request.Image);

            var entity = new Actor
            {
                FullName = request.FullName,
                Title = request.Title,
                ImageSrc = imageSrc
            };

            await _actorRepository.AddAsync(entity, cancellationToken);
            await _actorRepository.SaveAsync(cancellationToken);

            var dto = new ActorRequestDto
            {
                Id = entity.Id,
                FullName = entity.FullName,
                Title = entity.Title,
                ImageSrc = entity.ImageSrc
            };

            return dto;
        }
    }
}
