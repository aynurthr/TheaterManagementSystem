using MediatR;
using Theater.Application.Repositories;
using Theater.Domain.Models.Entities;
using System.Threading;
using System.Threading.Tasks;
using Theater.Application.Modules.GenreModule.Queries;

namespace Theater.Application.Modules.GenreModule.Commands.GenreAddCommand
{
    public class GenreAddRequestHandler : IRequestHandler<GenreAddRequest, GenreRequestDto>
    {
        private readonly IGenreRepository _genreRepository;

        public GenreAddRequestHandler(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<GenreRequestDto> Handle(GenreAddRequest request, CancellationToken cancellationToken)
        {
            var entity = new Genre
            {
                Name = request.Name
            };

            await _genreRepository.AddAsync(entity, cancellationToken);
            await _genreRepository.SaveAsync(cancellationToken);

            var dto = new GenreRequestDto
            {
                Id = entity.Id,
                Name = entity.Name
            };

            return dto;
        }
    }
}
