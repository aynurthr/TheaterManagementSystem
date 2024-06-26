using MediatR;
using Microsoft.AspNetCore.Mvc;
using Theater.Application.Modules.NewsModule.Queries.NewsGetAllQuery;
using Theater.Application.Modules.NewsModule.Queries.NewsGetByIdQuery;

namespace Theater.Presentation.Controllers
{
    public class NewsController : Controller
    {
        private readonly IMediator mediator;

        public NewsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IActionResult> Index(NewsGetAllRequest request)
        {
            var response = await mediator.Send(request);
            return View(response);
        }

        public async Task<IActionResult> Details(int id)
        {
            var request = new NewsGetByIdRequest { Id = id };
            var newsItem = await mediator.Send(request);

            //if (newsItem == null)
            //{
            //    return NotFound();
            //}

            return View(newsItem);
        }

    }
}
