using MediatR;
using Theater.Application.Repositories;
using Theater.Domain.Models.Entities;
using Theater.Infrastructure.Abstracts;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Theater.Application.Modules.PosterModule.Commands.PosterAddCommand
{
    public class PosterAddRequestHandler : IRequestHandler<PosterAddRequest, PosterAddRequestDto>
    {
        private readonly IPosterRepository _posterRepository;
        private readonly IActorRepository _actorRepository;
        private readonly IHallRepository _hallRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IFileService _fileService;

        public PosterAddRequestHandler(IPosterRepository posterRepository, IActorRepository actorRepository, IHallRepository hallRepository, IGenreRepository genreRepository, IFileService fileService)
        {
            _posterRepository = posterRepository;
            _actorRepository = actorRepository;
            _hallRepository = hallRepository;
            _genreRepository = genreRepository;
            _fileService = fileService;
        }

        public async Task<PosterAddRequestDto> Handle(PosterAddRequest request, CancellationToken cancellationToken)
        {
            var imageSrc = await _fileService.UploadAsync(request.Image);

            var genre = await _genreRepository.GetAll()
                .FirstOrDefaultAsync(g => g.Id == request.GenreId && g.DeletedAt == null, cancellationToken);

            if (genre == null)
            {
                throw new KeyNotFoundException("Genre not found.");
            }

            var entity = new Poster
            {
                Title = request.Title,
                GenreId = genre.Id,
                Duration = request.Duration,
                Age = request.Age,
                Description = request.Description,
                ImageSrc = imageSrc,
                Roles = request.Roles.Select(r => new Role
                {
                    RoleName = r.RoleName,
                    ActorId = r.ActorId
                }).ToList(),
                ShowDates = request.ShowDates.Select(sd => new ShowDate
                {
                    Date = sd.Date,
                    HallId = sd.HallId
                }).ToList()
            };

            await _posterRepository.AddAsync(entity, cancellationToken);
            await _posterRepository.SaveAsync(cancellationToken);

            var dto = new PosterAddRequestDto
            {
                Title = entity.Title,
                GenreId = entity.GenreId,
                Duration = entity.Duration,
                Age = entity.Age,
                Description = entity.Description,
                Image = request.Image,
                Roles = entity.Roles.Select(r => new RoleDto
                {
                    RoleName = r.RoleName,
                    ActorId = r.ActorId
                }).ToList(),
                ShowDates = entity.ShowDates.Select(sd => new ShowDateDto
                {
                    Date = sd.Date,
                    HallId = sd.HallId
                }).ToList()
            };

            return dto;
        }
    }
}
