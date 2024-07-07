using MediatR;
using Microsoft.EntityFrameworkCore;
using Theater.Application.Repositories;
using Theater.Infrastructure.Exceptions;

namespace Theater.Application.Modules.GenreModule.Commands.GenreRemoveCommand
{
    public class  GenreRemoveRequestHandler : IRequestHandler<GenreRemoveRequest, string>
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IPosterRepository _posterRepository;

        public GenreRemoveRequestHandler(IGenreRepository genreRepository, IPosterRepository posterRepository)
        {
            _genreRepository = genreRepository;
            _posterRepository = posterRepository;
        }

        public async Task<string> Handle(GenreRemoveRequest request, CancellationToken cancellationToken)
        {
            var entity = await _genreRepository.GetAsync(m => m.Id == request.Id && m.DeletedAt == null, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException("Genre entity not found.", request.Id);
            }

            var postersUsingGenre = await _posterRepository.GetAll(p => p.GenreId == request.Id && p.DeletedAt == null).ToListAsync(cancellationToken);

            if (postersUsingGenre.Any())
            {
                var count = postersUsingGenre.Count;
                return $"This genre is used by {count} poster(s). Change their genre to be able to delete this genre.";
            }

            _genreRepository.Remove(entity);
            await _genreRepository.SaveAsync(cancellationToken);
            return "success";
        }
    }
}
