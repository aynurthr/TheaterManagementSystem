using MediatR;
using Theater.Application.Repositories;
using System.Threading;
using System.Threading.Tasks;
using Theater.Infrastructure.Exceptions;

namespace Theater.Application.Modules.GenreModule.Commands.GenreRemoveCommand
{
    public class GenreRemoveRequestHandler : IRequestHandler<GenreRemoveRequest>
    {
        private readonly IGenreRepository _genreRepository;

        public GenreRemoveRequestHandler(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task Handle(GenreRemoveRequest request, CancellationToken cancellationToken)
        {
            var entity = await _genreRepository.GetAsync(m => m.Id == request.Id && m.DeletedAt == null, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException("Genre entity not found.", request.Id);
            }

            _genreRepository.Remove(entity);
            await _genreRepository.SaveAsync(cancellationToken);
        }
    }
}
