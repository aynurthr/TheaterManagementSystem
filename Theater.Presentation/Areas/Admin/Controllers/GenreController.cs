﻿using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Theater.Application.Modules.GenreModule.Commands.GenreAddCommand;
using Theater.Application.Modules.GenreModule.Commands.GenreEditCommand;
using Theater.Application.Modules.GenreModule.Commands.GenreRemoveCommand;
using Theater.Application.Modules.GenreModule.Queries;
using Theater.Application.Modules.GenreModule.Queries.GenreGetAllQuery;
using Theater.Application.Modules.GenreModule.Queries.GenreGetByIdQuery;

namespace Theater.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("genres.manage")]

    public class GenreController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IValidator<GenreAddRequest> _genreAddValidator;
        private readonly IValidator<GenreEditRequest> _genreEditValidator;

        public GenreController(IMediator mediator, IValidator<GenreAddRequest> genreAddValidator, IValidator<GenreEditRequest> genreEditValidator)
        {
            _mediator = mediator;
            _genreAddValidator = genreAddValidator;
            _genreEditValidator = genreEditValidator;

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
            if (response == null)
            {
                return View("NotFound");
            }

            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] GenreEditRequest request)
        {
            var validationResult = await _genreEditValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                var model = new GenreRequestDto
                {
                    Id = request.Id,
                    Name = request.Name
                };

                return View(model);
            }

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
