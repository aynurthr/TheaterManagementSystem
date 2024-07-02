using MediatR;
using Theater.Application.Repositories;
using System.Threading;
using System.Threading.Tasks;
using Theater.Application.Modules.GenreModule.Queries;

namespace Theater.Application.Modules.GenreModule.Commands.GenreEditCommand
{
    public class GenreEditRequestHandler : IRequestHandler<GenreEditRequest, GenreRequestDto>
    {
        private readonly IGenreRepository _genreRepository;

        public GenreEditRequestHandler(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<GenreRequestDto> Handle(GenreEditRequest request, CancellationToken cancellationToken)
        {
            var entity = await _genreRepository.GetAsync(m => m.Id == request.Id && m.DeletedAt == null, cancellationToken);

            entity.Name = request.Name;

            _genreRepository.Edit(entity);
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
