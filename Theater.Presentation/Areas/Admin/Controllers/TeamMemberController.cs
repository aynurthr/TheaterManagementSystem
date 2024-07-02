using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Theater.Application.Modules.TeamMemberModule.Commands.TeamMemberAddCommand;
using Theater.Application.Modules.TeamMemberModule.Commands.TeamMemberEditCommand;
using Theater.Application.Modules.TeamMemberModule.Commands.TeamMemberRemoveCommand;
using Theater.Application.Modules.TeamMemberModule.Queries.TeamMemberGetAllQuery;
using Theater.Application.Modules.TeamMemberModule.Queries.TeamMemberGetByIdQuery;

namespace Theater.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeamMemberController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IValidator<TeamMemberAddRequest> _teamMemberAddValidator;

        public TeamMemberController(IMediator mediator, IValidator<TeamMemberAddRequest> teamMemberAddValidator)
        {
            _mediator = mediator;
            _teamMemberAddValidator = teamMemberAddValidator;
        }

        public async Task<IActionResult> Index(TeamMemberGetAllRequest request)
        {
            var response = await _mediator.Send(request);
            return View(response);
        }

        public async Task<IActionResult> Details([FromRoute] TeamMemberGetByIdRequest request)
        {
            var response = await _mediator.Send(request);
            return View(response);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] TeamMemberAddRequest request)
        {
            var validationResult = await _teamMemberAddValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return View(request);
            }

            await _mediator.Send(request);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit([FromRoute] TeamMemberGetByIdRequest request)
        {
            var response = await _mediator.Send(request);
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] TeamMemberEditRequest request)
        {
            await _mediator.Send(request);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Remove([FromRoute] TeamMemberRemoveRequest request)
        {
            await _mediator.Send(request);
            return RedirectToAction(nameof(Index));
        }
    }
}
