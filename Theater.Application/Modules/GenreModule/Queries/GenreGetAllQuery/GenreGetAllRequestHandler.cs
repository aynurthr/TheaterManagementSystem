using MediatR;
using Microsoft.EntityFrameworkCore;
using Theater.Application.Repositories;

namespace Theater.Application.Modules.GenreModule.Queries.GenreGetAllQuery
{
    public class GenreGetAllRequestHandler : IRequestHandler<GenreGetAllRequest, IEnumerable<GenreRequestDto>>
    {
        private readonly IGenreRepository _genreRepository;

        public GenreGetAllRequestHandler(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<IEnumerable<GenreRequestDto>> Handle(GenreGetAllRequest request, CancellationToken cancellationToken)
        {
            var query = _genreRepository.GetAll();

            if (request.OnlyAvailable)
            {
                query = query.Where(m => m.DeletedAt == null);
            }

            var queryResponse = await query
                .OrderBy(m => m.Name)
                .Select(m => new GenreRequestDto
                {
                    Id = m.Id,
                    Name = m.Name
                }).ToListAsync(cancellationToken);

            return queryResponse;
        }
    }
}
