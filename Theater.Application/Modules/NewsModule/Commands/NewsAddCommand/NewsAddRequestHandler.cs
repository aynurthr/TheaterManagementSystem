using MediatR;
using Theater.Application.Repositories;
using Theater.Domain.Models.Entities;
using Theater.Infrastructure.Abstracts;

namespace Theater.Application.Modules.NewsModule.Commands.NewsAddCommand
{
    public class NewsAddRequestHandler : IRequestHandler<NewsAddRequest, NewsAddRequestDto>
    {
        private readonly INewsRepository _newsRepository;
        private readonly IFileService _fileService;

        public NewsAddRequestHandler(INewsRepository newsRepository, IFileService fileService)
        {
            _newsRepository = newsRepository;
            _fileService = fileService;
        }

        public async Task<NewsAddRequestDto> Handle(NewsAddRequest request, CancellationToken cancellationToken)
        {
            var entity = new News
            {
                Title = request.Title,
                Date = request.Date,
                Description = request.Description
            };

            if (request.Image != null)
            {
                entity.ImageSrc = await _fileService.UploadAsync(request.Image);
            }

            await _newsRepository.AddAsync(entity, cancellationToken);
            await _newsRepository.SaveAsync(cancellationToken);

            var dto = new NewsAddRequestDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Date = entity.Date,
                Description = entity.Description,
                Image = entity.ImageSrc
            };

            return dto;
        }
    }
}
