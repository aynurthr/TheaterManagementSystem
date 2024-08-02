using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Theater.Application.Modules.ActorModule.Commands.ActorAddCommand;
using Theater.Application.Modules.ActorModule.Commands.ActorEditCommand;
using Theater.Application.Modules.ActorModule.Commands.ActorRemoveCommand;
using Theater.Application.Modules.ActorModule.Queries.ActorGetAllQuery;
using Theater.Application.Modules.ActorModule.Queries.ActorGetByIdQuery;
using Microsoft.AspNetCore.Authorization;
using Theater.Application.Modules.ActorModule.Queries;
using Microsoft.Extensions.Hosting;
using static Dapper.SqlMapper;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Theater.Application.Repositories;

namespace Theater.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("actors.manage")]
    public class ActorController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IValidator<ActorAddRequest> _actorAddValidator;
        private readonly IValidator<ActorEditRequest> _actorEditValidator;
        private readonly IActionContextAccessor _ctx;


        public ActorController(IActionContextAccessor ctx,IMediator mediator, IValidator<ActorAddRequest> actorAddValidator, IValidator<ActorEditRequest> actorEditValidator)
        {
            _mediator = mediator;
            _actorAddValidator = actorAddValidator;
            _actorEditValidator = actorEditValidator;
            _ctx = ctx;
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
            var validationResult = await _actorEditValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                string host = $"{_ctx.ActionContext.HttpContext.Request.Scheme}://{_ctx.ActionContext.HttpContext.Request.Host}";
                var model = new ActorRequestDto
                {
                    Id = request.Id,
                    FullName = request.FullName,
                    Title = request.Title,
                    ImageSrc = $"{host}/uploads/images/{request.ImageUrl}",
                };

                return View(model);
            }

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
