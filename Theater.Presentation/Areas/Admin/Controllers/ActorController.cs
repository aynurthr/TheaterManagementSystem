using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Theater.Application.Modules.ActorModule.Commands.ActorAddCommand;
using Theater.Application.Modules.ActorModule.Commands.ActorEditCommand;
using Theater.Application.Modules.ActorModule.Commands.ActorRemoveCommand;
using Theater.Application.Modules.ActorModule.Queries.ActorGetAllQuery;
using Theater.Application.Modules.ActorModule.Queries.ActorGetByIdQuery;

namespace Theater.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ActorController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IValidator<ActorAddRequest> _actorAddValidator;

        public ActorController(IMediator mediator, IValidator<ActorAddRequest> actorAddValidator)
        {
            _mediator = mediator;
            _actorAddValidator = actorAddValidator;
        }

        public async Task<IActionResult> Index(ActorGetAllRequest request)
        {
            var response = await _mediator.Send(request);
            return View(response);
        }

        public async Task<IActionResult> Details([FromRoute] ActorGetByIdRequest request)
        {
            var response = await _mediator.Send(request);
            return View(response);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ActorAddRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            await _mediator.Send(request);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit([FromRoute] ActorGetByIdRequest request)
        {
            var response = await _mediator.Send(request);
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] ActorEditRequest request)
        {
            await _mediator.Send(request);
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> Remove([FromRoute] ActorRemoveRequest request)
        {
            var response = await _mediator.Send(request);
            return Json(new { message = response });
        }

    }
}
