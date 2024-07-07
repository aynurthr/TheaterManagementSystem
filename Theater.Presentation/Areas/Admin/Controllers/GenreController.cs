using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Theater.Application.Modules.GenreModule.Commands.GenreAddCommand;
using Theater.Application.Modules.GenreModule.Commands.GenreEditCommand;
using Theater.Application.Modules.GenreModule.Commands.GenreRemoveCommand;
using Theater.Application.Modules.GenreModule.Queries.GenreGetAllQuery;
using Theater.Application.Modules.GenreModule.Queries.GenreGetByIdQuery;

namespace Theater.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GenreController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IValidator<GenreAddRequest> _genreAddValidator;

        public GenreController(IMediator mediator, IValidator<GenreAddRequest> genreAddValidator)
        {
            _mediator = mediator;
            _genreAddValidator = genreAddValidator;
        }

        public async Task<IActionResult> Index(GenreGetAllRequest request)
        {
            var response = await _mediator.Send(request);
            return View(response);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] GenreAddRequest request)
        {
            var validationResult = await _genreAddValidator.ValidateAsync(request);

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

        public async Task<IActionResult> Edit([FromRoute] GenreGetByIdRequest request)
        {
            var response = await _mediator.Send(request);
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] GenreEditRequest request)
        {
            await _mediator.Send(request);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Remove([FromRoute] GenreRemoveRequest request)
        {
            var response = await _mediator.Send(request);
            return Json(new { message = response });
        }

    }
}
