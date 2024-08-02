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
using Microsoft.AspNetCore.Authorization;
using System.Threading;

namespace Theater.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("tickets.manage")]

    public class ShowDateController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IShowDateRepository _showDateRepository;
        private readonly ISeatRepository _seatRepository;


        public ShowDateController(IMediator mediator, IShowDateRepository showDateRepository, ISeatRepository seatRepository)
        {
            _mediator = mediator;
            _showDateRepository = showDateRepository;
            _seatRepository = seatRepository;

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
                return View("NotFound");
            }

            // Fetching the show date and related poster details
            var showDate = await _showDateRepository.GetAll()
                .Include(sd => sd.Poster)
                .FirstOrDefaultAsync(sd => sd.Id == id && sd.DeletedAt == null);

            if (response == null)
            {
                return View("NotFound");
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
                return View("NotFound");
            }

            // Fetching the show date and related poster details
            var showDate = await _showDateRepository.GetAll()
                .Include(sd => sd.Poster)
                .FirstOrDefaultAsync(sd => sd.Id == showDateId && sd.DeletedAt == null);

            if (showDate == null)
            {
                return View("NotFound");
            }

            var seat = await _seatRepository.GetAll()
                .FirstOrDefaultAsync(s => s.Id == seatId && s.DeletedAt == null);

            if (seat == null)
            {
                return View("NotFound");
            }

            ViewBag.PosterTitle = showDate.Poster.Title;
            ViewBag.ShowDate = showDate.Date.ToString("dd MMM yyyy HH:mm");
            ViewBag.SeatDetails = $"{seat.Row}{seat.Number}";

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
