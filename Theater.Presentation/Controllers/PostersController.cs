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
        private readonly ILogger<PostersController> _logger;


        public PostersController(IMediator mediator, ILogger<PostersController> logger)
        {
            _mediator = mediator;
            _logger = logger;

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
            _logger.LogInformation("BuyTicket called with PosterId: {PosterId}", id);
            var request = new PosterBuyTicketRequest { PosterId = id };
            var response = await _mediator.Send(request);

            if (response == null)
            {
                _logger.LogWarning("No response found for PosterId: {PosterId}", id);
                return NotFound();
            }

            _logger.LogInformation("Returning view for BuyTicket with PosterId: {PosterId}", id);
            return View(response);
        }

        [HttpGet("api/tickets/{showDateId}")]
        public async Task<IActionResult> GetTickets(int showDateId)
        {
            _logger.LogInformation("GetTickets called with ShowDateId: {ShowDateId}", showDateId);
            var request = new PosterBuyTicketRequest { ShowDateId = showDateId };
            var response = await _mediator.Send(request);

            if (response == null)
            {
                _logger.LogWarning("No tickets found for ShowDateId: {ShowDateId}", showDateId);
                return NotFound();
            }

            _logger.LogInformation("Returning JSON for GetTickets with ShowDateId: {ShowDateId}", showDateId);
            return Json(response);
        }
    }
}

