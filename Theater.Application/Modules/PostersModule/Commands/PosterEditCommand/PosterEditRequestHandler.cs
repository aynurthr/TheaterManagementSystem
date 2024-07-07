using MediatR;
using Theater.Application.Repositories;
using Theater.Domain.Models.Entities;
using Theater.Infrastructure.Abstracts;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Theater.Application.Modules.PosterModule.Commands.PosterEditCommand
{
    public class PosterEditRequestHandler : IRequestHandler<PosterEditRequest, bool>
    {
        private readonly IPosterRepository _posterRepository;
        private readonly IFileService _fileService;

        public PosterEditRequestHandler(IPosterRepository posterRepository, IFileService fileService)
        {
            _posterRepository = posterRepository;
            _fileService = fileService;
        }

        public async Task<bool> Handle(PosterEditRequest request, CancellationToken cancellationToken)
        {
            var entity = await _posterRepository.GetAsync(p => p.Id == request.Id && p.DeletedAt == null, cancellationToken);

            if (entity == null)
            {
                return false;
            }

            entity.Title = request.Title;
            entity.GenreId = request.GenreId;
            entity.Duration = request.Duration;
            entity.Age = request.Age;
            entity.Description = request.Description;

            if (request.Image != null)
            {
                entity.ImageSrc = await _fileService.ChangeFileAsync(entity.ImageSrc, request.Image);
            }

            // Update show dates
            foreach (var showDate in request.ShowDates)
            {
                var existingShowDate = entity.ShowDates.FirstOrDefault(sd => sd.Id == showDate.ShowDateId);
                if (existingShowDate != null)
                {
                    existingShowDate.Date = showDate.Date;
                }
            }

            _posterRepository.Edit(entity);
            await _posterRepository.SaveAsync(cancellationToken);

            return true;
        }
    }
}
