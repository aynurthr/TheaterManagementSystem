using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Theater.Application.Repositories;

namespace Theater.Application.Modules.NewsModule.Queries.NewsGetByIdQuery
{
    public class NewsGetByIdRequestHandler : IRequestHandler<NewsGetByIdRequest, NewsGetByIdRequestDto>
    {
        private readonly INewsRepository newsRepository;
        private readonly IActionContextAccessor ctx;

        public NewsGetByIdRequestHandler(INewsRepository newsRepository, IActionContextAccessor ctx)
        {
            this.newsRepository = newsRepository;
            this.ctx = ctx;
        }

        public async Task<NewsGetByIdRequestDto> Handle(NewsGetByIdRequest request, CancellationToken cancellationToken)
        {
            var entity = await newsRepository.GetAsync(m => m.Id == request.Id && m.DeletedAt == null, cancellationToken);

            string host = $"{ctx.ActionContext.HttpContext.Request.Scheme}://{ctx.ActionContext.HttpContext.Request.Host}";

            return new NewsGetByIdRequestDto
            {
                Id = entity.Id,
                Title = entity.Title,
                ImageUrl = $"{host}/uploads/images/{entity.ImageSrc}",
                Description = entity.Description,
                PublishedAt = entity.Date
            };
        }

    }
}

