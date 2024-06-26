using Theater.Application.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Theater.Application.Modules.NewsModule.Queries.NewsGetAllQuery
{
    class NewsGetAllRequestHandler : IRequestHandler<NewsGetAllRequest, IEnumerable<NewsGetAllRequestDto>>
    {
        private readonly INewsRepository newsRepository;
        private readonly IActionContextAccessor ctx;

        public NewsGetAllRequestHandler(INewsRepository newsRepository, IActionContextAccessor ctx)
        {
            this.newsRepository = newsRepository;
            this.ctx = ctx;
        }

        public async Task<IEnumerable<NewsGetAllRequestDto>> Handle(NewsGetAllRequest request, CancellationToken cancellationToken)
        {
            var query = newsRepository.GetAll();

            if (request.OnlyAvailable)
            {
                query = query.Where(m => m.DeletedAt == null);
            }

            string host = $"{ctx.ActionContext.HttpContext.Request.Scheme}://{ctx.ActionContext.HttpContext.Request.Host}";
            var queryResponse = await query
                .OrderByDescending(m => m.Date) // Ensure the items are sorted by PublishedAt in descending order
                .Select(m => new NewsGetAllRequestDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    ImageUrl = $"{host}/uploads/images/{m.ImageSrc}",
                    Description = m.Description,
                    PublishedAt = m.Date // Assuming PublishedAt is the property in your News entity
                }).ToListAsync(cancellationToken);

            return queryResponse;
        }
    }
}
