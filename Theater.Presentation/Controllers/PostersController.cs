using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Theater.Application.Modules.PosterModule.Queries.PosterBuyTicketRequestDto;
using Theater.Application.Modules.PosterModule.Queries.PosterGetAllQuery;
using Theater.Application.Modules.PosterModule.Queries.PosterGetByIdQuery;

namespace Theater.Presentation.Controllers
{
    public class PostersController : Controller
    {
        private readonly IMediator mediator;

        public PostersController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IActionResult> Index(PosterGetAllRequest request)
        {
            var response = await mediator.Send(request);
            return View(response);
        }

        public async Task<IActionResult> Details(int id)
        {
            var request = new PosterGetByIdRequest { Id = id };
            var response = await mediator.Send(request);

            if (response == null)
            {
                return NotFound();
            }

            return View(response);
        }

        [HttpGet("posters/buyticket/{posterId}")]
        public async Task<IActionResult> BuyTicket(int posterId)
        {
            var request = new PosterBuyTicketRequest { PosterId = posterId };
            var response = await mediator.Send(request);

            if (response == null)
            {
                return NotFound();
            }

            return View(response);
        }
    }
}
