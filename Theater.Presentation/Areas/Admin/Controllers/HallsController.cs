using MediatR;
using Microsoft.AspNetCore.Mvc;
using Theater.Application.Modules.HallModule.Queries.HallGetByIdQuery;
using Theater.Application.Modules.SeatModule.Commands.AddRemoveSeatsCommand;
using Theater.Application.Modules.HallModule.Queries.HallGetAllQuery;
using Theater.Application.Modules.SeatModule.Queries;

namespace Theater.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    [ApiController]
    public class HallsController : Controller
    {
        private readonly IMediator _mediator;

        public HallsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var request = new HallGetAllRequest();
            var response = await _mediator.Send(request);

            if (response == null)
            {
                return NotFound();
            }

            return View(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var request = new HallGetByIdRequest { Id = id };
            var response = await _mediator.Send(request);

            if (response == null)
            {
                return NotFound();
            }

            return View(response);
        }

        [HttpPost("add-remove-seats")]
        public async Task<IActionResult> AddRemoveSeats([FromBody] AddRemoveSeatsRequest request)
        {
            if (request.SeatsToAdd == null)
            {
                request.SeatsToAdd = new List<SeatRequestDto>();
            }

            if (request.SeatsToRemove == null)
            {
                request.SeatsToRemove = new List<SeatRequestDto>();
            }

            var result = await _mediator.Send(request);

            if (!result)
            {
                return BadRequest("Failed to update seats.");
            }

            return Ok("Seats updated successfully.");
        }
    }
}
