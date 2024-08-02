using MediatR;
using Theater.Application.Repositories;
using Theater.Domain.Models.Entities;
using Theater.Infrastructure.Abstracts;
using System.Threading;
using System.Threading.Tasks;
using Theater.Application.Modules.ActorModule.Queries;
using FluentValidation;

namespace Theater.Application.Modules.ActorModule.Commands.ActorEditCommand
{
    public class ActorEditRequestHandler : IRequestHandler<ActorEditRequest, ActorRequestDto>
    {
        private readonly IActorRepository _actorRepository;
        private readonly IFileService _fileService;

        public ActorEditRequestHandler(IActorRepository actorRepository, IFileService fileService)
        {
            _actorRepository = actorRepository;
            _fileService = fileService;
        }

        public async Task<ActorRequestDto> Handle(ActorEditRequest request, CancellationToken cancellationToken)
        {
            var entity = await _actorRepository.GetAsync(m => m.Id == request.Id && m.DeletedAt == null, cancellationToken);

            entity.FullName = request.FullName;
            entity.Title = request.Title;

            if (request.Image != null)
            {
                entity.ImageSrc = await _fileService.ChangeFileAsync(entity.ImageSrc, request.Image);
                request.ImageUrl = entity.ImageSrc;  // Set the Image URL here
            }

            _actorRepository.Edit(entity);
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