using MediatR;

namespace Theater.Application.Modules.PosterModule.Queries.PosterGetByIdQuery
{
    public class PosterGetByIdRequest : IRequest<PosterGetByIdRequestDto>
    {
        public int Id { get; set; }
    }
}