using MediatR;

namespace Theater.Application.Modules.NewsModule.Queries.NewsGetAllQuery
{
    public class NewsGetAllRequest : IRequest<IEnumerable<NewsGetAllRequestDto>>
    {
        public bool OnlyAvailable { get; set; } = true;
    }
}
