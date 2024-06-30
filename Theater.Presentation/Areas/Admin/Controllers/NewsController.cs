using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Theater.Application.Modules.NewsModule.Commands.NewsAddCommand;
using Theater.Application.Modules.NewsModule.Commands.NewsEditCommand;
using Theater.Application.Modules.NewsModule.Commands.NewsRemoveCommand;
using Theater.Application.Modules.NewsModule.Queries.NewsGetAllQuery;
using Theater.Application.Modules.NewsModule.Queries.NewsGetByIdQuery;

namespace Theater.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NewsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IValidator<NewsAddRequest> _newsAddValidator;

        public NewsController(IMediator mediator, IValidator<NewsAddRequest> newsAddValidator)
        {
            _mediator = mediator;
            _newsAddValidator = newsAddValidator;
        }

        public async Task<IActionResult> Index(NewsGetAllRequest request)
        {
            var response = await _mediator.Send(request);
            return View(response);
        }

        public async Task<IActionResult> Details([FromRoute] NewsGetByIdRequest request)
        {
            var response = await _mediator.Send(request);
            return View(response);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] NewsAddRequest request)
        {
            var validationResult = await _newsAddValidator.ValidateAsync(request);

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

        public async Task<IActionResult> Edit([FromRoute] NewsGetByIdRequest request)
        {
            var response = await _mediator.Send(request);
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] NewsEditRequest request)
        {
            await _mediator.Send(request);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Remove([FromRoute] NewsRemoveRequest request)
        {
            await _mediator.Send(request);
            return RedirectToAction(nameof(Index));
        }
    }
}
