using MediatR;

namespace Theater.Application.Modules.NewsModule.Queries.NewsGetByIdQuery
{
    public class NewsGetByIdRequest : IRequest<NewsGetByIdRequestDto>
    {
        public int Id { get; set; }
    }
}
