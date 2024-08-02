using MediatR;
using Theater.Application.Repositories;

namespace Theater.Application.Modules.GenreModule.Queries.GenreGetByIdQuery
{
    public class GenreGetByIdRequestHandler :IRequestHandler<GenreGetByIdRequest, GenreRequestDto>
    {
        private readonly IGenreRepository genreRepository;

        public GenreGetByIdRequestHandler(IGenreRepository genreRepository)
        {
            this.genreRepository = genreRepository;
        }

        public async Task<GenreRequestDto> Handle(GenreGetByIdRequest request, CancellationToken cancellationToken)
        {
            var entity = await genreRepository.GetAsync(m => m.Id == request.Id && m.DeletedAt == null, cancellationToken);

            if (entity == null)
            {
                return null;
            }
            return new GenreRequestDto
            {
                Id = entity.Id,
                Name = entity.Name,
            };
        }
    }
}