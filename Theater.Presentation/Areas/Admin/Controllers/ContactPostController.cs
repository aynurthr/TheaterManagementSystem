using MediatR;
using Microsoft.AspNetCore.Mvc;
using Theater.Application.Modules.ContactPostModule.Queries.ContactPostGetAllQuery;
using System.Threading.Tasks;
using Theater.Application.Modules.ContactPostModule.Commands.ContactPostReplyCommand;
using Theater.Application.Modules.ContactPostModule.Queries.ContactPostGetByIdQuery;

namespace Theater.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactPostController : Controller
    {
        private readonly IMediator _mediator;

        public ContactPostController(IMediator mediator)
        {
            _mediator = mediator;
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
                return View(request);
            }

            var replyRequest = new ContactPostReplyRequest
            {
                Id = request.Id,
                ReplyMessage = request.ReplyMessage
            };

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
