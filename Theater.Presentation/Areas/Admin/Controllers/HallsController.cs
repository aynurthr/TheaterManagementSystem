using MediatR;
using Microsoft.AspNetCore.Mvc;
using Theater.Application.Modules.HallModule.Queries.HallGetByIdQuery;
using Theater.Application.Modules.SeatModule.Commands.AddRemoveSeatsCommand;
using Theater.Application.Modules.HallModule.Queries.HallGetAllQuery;
using Theater.Application.Modules.SeatModule.Queries;
using Theater.Application.Modules.HallModule.Commands.HallAddCommand;
using Microsoft.AspNetCore.Authorization;
using FluentValidation;
using Theater.Application.Modules.GenreModule.Commands.GenreAddCommand;

namespace Theater.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    [Authorize("halls.manage")]
    public class HallsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IValidator<HallAddRequest> _hallAddValidator;

        public HallsController(IMediator mediator, IValidator<HallAddRequest> hallAddValidator)
        {
            _mediator = mediator;
            _hallAddValidator = hallAddValidator;
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
                return View("NotFound");
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

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] HallAddRequest request)
        {
            var validationResult = await _hallAddValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return View(request);
            }

            var result = await _mediator.Send(request);

            return RedirectToAction(nameof(Index));
        }
    }
}
