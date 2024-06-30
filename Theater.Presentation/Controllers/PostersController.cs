using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Theater.Application.Modules.PosterModule.Queries.PosterGetAllQuery;
using Theater.Application.Modules.PosterModule.Queries.PosterGetByIdQuery;
using Theater.Application.Modules.PosterModule.Queries.PosterBuyTicketQuery;

namespace Theater.Presentation.Controllers
{
    public class PostersController : Controller
    {
        private readonly IMediator _mediator;

        public PostersController(IMediator mediator, DbContext context)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index(PosterGetAllRequest request)
        {
            var response = await _mediator.Send(request);
            return View(response);
        }

        public async Task<IActionResult> Details(int id)
        {
            var request = new PosterGetByIdRequest { Id = id };
            var response = await _mediator.Send(request);

            if (response == null)
            {
                return NotFound();
            }

            return View(response);
        }

        public async Task<IActionResult> BuyTicket(int id)
        {
            var request = new PosterBuyTicketRequest { PosterId = id };
            var response = await _mediator.Send(request);

            if (response == null)
            {
                return NotFound();
            }

            return View(response);
        }

        [HttpGet("/api/tickets/{showDateId}")]
        public async Task<IActionResult> GetTickets(int showDateId)
        {
            var request = new PosterBuyTicketRequest { ShowDateId = showDateId };
            var response = await _mediator.Send(request);

            if (response == null)
            {
                return NotFound();
            }

            return Json(response);
        }
    }
}

