﻿using MediatR;
using Theater.Application.Repositories;
using Theater.Domain.Models.Entities;
using Theater.Infrastructure.Abstracts;
using System.Threading;
using System.Threading.Tasks;

namespace Theater.Application.Modules.NewsModule.Commands.NewsEditCommand
{
    public class NewsEditRequestHandler : IRequestHandler<NewsEditRequest, News>
    {
        private readonly INewsRepository _newsRepository;
        private readonly IFileService _fileService;

        public NewsEditRequestHandler(INewsRepository newsRepository, IFileService fileService)
        {
            _newsRepository = newsRepository;
            _fileService = fileService;
        }

        public async Task<News> Handle(NewsEditRequest request, CancellationToken cancellationToken)
        {
            var entity = await _newsRepository.GetAsync(m => m.Id == request.Id && m.DeletedAt == null, cancellationToken);

            entity.Title = request.Title;
            entity.Description = request.Description;
            entity.Date = request.Date;

            if (request.Image != null)
                entity.ImageSrc = await _fileService.ChangeFileAsync(entity.ImageSrc, request.Image);

            _newsRepository.Edit(entity);
            await _newsRepository.SaveAsync(cancellationToken);

            return entity;
        }
    }
}
