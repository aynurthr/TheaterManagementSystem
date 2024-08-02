using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Theater.Application.Modules.ActorModule.Queries.ActorGetAllQuery;
using Theater.Application.Modules.GenreModule.Queries.GenreGetAllQuery;
using Theater.Application.Modules.HallModule.Queries.HallGetAllQuery;
using Theater.Application.Modules.PosterModule.Commands.PosterAddCommand;
using Theater.Application.Modules.PosterModule.Commands.PosterEditCommand;
using Theater.Application.Modules.PosterModule.Commands.PosterRemoveCommand;
using Theater.Application.Modules.PosterModule.Queries.PosterGetAllQuery;
using Theater.Application.Modules.PosterModule.Queries.PosterGetByIdQuery;

namespace Theater.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("posters.manage")]

    public class PosterController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IValidator<PosterAddRequest> _posterAddValidator;
        private readonly IValidator<PosterEditRequest> _posterEditValidator;

        public PosterController(IMediator mediator, IValidator<PosterAddRequest> posterAddValidator, IValidator<PosterEditRequest> posterEditValidator)
        {
            _mediator = mediator;
            _posterAddValidator = posterAddValidator;
            _posterEditValidator = posterEditValidator;
        }

        public async Task<IActionResult> Index(PosterGetAllRequest request)
        {
            var response = await _mediator.Send(request);
            return View(response);
        }

        public async Task<IActionResult> Details([FromRoute] PosterGetByIdRequest request)
        {
            var response = await _mediator.Send(request);
            if (response == null)
            {
                return View("NotFound");
            }
            return View(response);
        }

        public async Task<IActionResult> Create()
        {
            await PrepareCreateEditViewData();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] PosterAddRequest request)
        {
            var validationResult = await _posterAddValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                await PrepareCreateEditViewData();
                return View(request);
            }

            await _mediator.Send(request);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit([FromRoute] PosterGetByIdRequest request)
        {
            var response = await _mediator.Send(request);
            if (response == null)
            {
                return View("NotFound");
            }

            await PrepareCreateEditViewData();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] PosterEditRequest request)
        {
            var validationResult = await _posterEditValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                await PrepareCreateEditViewData();
                return View(request);
            }

            await _mediator.Send(request);
            return RedirectToAction(nameof(Index));
        }

        private async Task PrepareCreateEditViewData()
        {
            var genres = await _mediator.Send(new GenreGetAllRequest());
            var actors = await _mediator.Send(new ActorGetAllRequest());
            var halls = await _mediator.Send(new HallGetAllRequest());

            ViewBag.Genres = genres;
            ViewBag.Actors = actors;
            ViewBag.Halls = halls;
        }

        [HttpPost]
        public async Task<IActionResult> Remove([FromRoute] PosterRemoveRequest request)
        {
            await _mediator.Send(request);
            return RedirectToAction(nameof(Index));
        }
    }
}
