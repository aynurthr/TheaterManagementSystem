using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Theater.Application.Modules.ShowDateModule.Commands.UpsertTicketCommand;
using Theater.Application.Modules.ShowDateModule.Queries;
using Theater.Application.Modules.ShowDateModule.Queries.GetSeatsByShowDateQuery;
using Theater.Application.Modules.ShowDateModule.Queries.GetTicketBySeatAndShowDateQuery;
using Theater.Application.Modules.TeamMemberModule.Queries.TeamMemberGetAllQuery;
using Theater.Application.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Theater.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ShowDateController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IShowDateRepository _showDateRepository;

        public ShowDateController(IMediator mediator, IShowDateRepository showDateRepository)
        {
            _mediator = mediator;
            _showDateRepository = showDateRepository;
        }

        public async Task<IActionResult> Index(ShowDateGetAllRequest request)
        {
            var response = await _mediator.Send(request);
            return View(response);
        }

        public async Task<IActionResult> Details(int id)
        {
            var request = new GetSeatsByShowDateRequest { ShowDateId = id };
            var response = await _mediator.Send(request);

            if (response == null)
            {
                return NotFound();
            }

            // Fetching the show date and related poster details
            var showDate = await _showDateRepository.GetAll()
                .Include(sd => sd.Poster)
                .FirstOrDefaultAsync(sd => sd.Id == id && sd.DeletedAt == null); // Adjust this to match your repository setup

            if (showDate == null)
            {
                return NotFound();
            }

            ViewBag.PosterTitle = showDate.Poster.Title;
            ViewBag.ShowDate = showDate.Date.ToString("dd MMM yyyy HH:mm");
            ViewBag.ShowDateId = id;

            return View(response);
        }


        [HttpGet]
        public async Task<IActionResult> UpsertTicket(int showDateId, int seatId)
        {
            var request = new GetTicketBySeatAndShowDateRequest
            {
                ShowDateId = showDateId,
                SeatId = seatId
            };

            var response = await _mediator.Send(request);

            if (response == null)
            {
                response = new TicketDto
                {
                    ShowDateId = showDateId,
                    SeatId = seatId,
                    Price = 0 // Default price for new ticket
                };
            }

            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpsertTicket(UpsertTicketRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            await _mediator.Send(request);
            return RedirectToAction("Details", new { id = request.ShowDateId });
        }
    }
}
