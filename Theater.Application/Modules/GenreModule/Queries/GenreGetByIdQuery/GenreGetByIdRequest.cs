using MediatR;

namespace Theater.Application.Modules.GenreModule.Queries.GenreGetByIdQuery
{
    public class GenreGetByIdRequest : IRequest<GenreRequestDto>
    {
        public int Id { get; set; }
    }
}
