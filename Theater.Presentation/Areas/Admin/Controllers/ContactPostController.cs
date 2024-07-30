using MediatR;
using Microsoft.AspNetCore.Mvc;
using Theater.Application.Modules.ContactPostModule.Queries.ContactPostGetAllQuery;
using System.Threading.Tasks;
using Theater.Application.Modules.ContactPostModule.Commands.ContactPostReplyCommand;
using Theater.Application.Modules.ContactPostModule.Queries.ContactPostGetByIdQuery;
using FluentValidation;

namespace Theater.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactPostController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IValidator<ContactPostReplyRequest> _replyValidator;

        public ContactPostController(IMediator mediator, IValidator<ContactPostReplyRequest> replyValidator)
        {
            _mediator = mediator;
            _replyValidator = replyValidator;
        }

        public async Task<IActionResult> Index()
        {
            var request = new ContactPostGetAllRequest();
            var response = await _mediator.Send(request);
            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> Reply(int id)
        {
            var request = new ContactPostGetByIdRequest { Id = id };
            var response = await _mediator.Send(request);
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Reply(ContactPostGetByIdRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                // Re-fetch the details to populate the view correctly
                var detailsRequest = new ContactPostGetByIdRequest { Id = request.Id };
                var detailsResponse = await _mediator.Send(detailsRequest);
                detailsResponse.ReplyMessage = request.ReplyMessage;

                return View(detailsResponse);
            }

            var replyRequest = new ContactPostReplyRequest
            {
                Id = request.Id,
                ReplyMessage = request.ReplyMessage
            };

            var validationResult = await _replyValidator.ValidateAsync(replyRequest);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                // Re-fetch the details to populate the view correctly
                var detailsRequest = new ContactPostGetByIdRequest { Id = request.Id };
                var detailsResponse = await _mediator.Send(detailsRequest);
                detailsResponse.ReplyMessage = request.ReplyMessage;

                return View(detailsResponse);
            }

            await _mediator.Send(replyRequest);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            var request = new ContactPostGetByIdRequest { Id = id };
            var response = await _mediator.Send(request);
            return View(response);
        }
    }
}
