using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using Theater.Application.Modules.NewsletterModule.Commands.NewsletterSubscribeCommand;

namespace Theater.Presentation.Controllers
{
    [Route("[controller]")]
    [AllowAnonymous]
    public class NewsletterController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IValidator<NewsletterSubscribeRequest> _validator;

        public NewsletterController(IMediator mediator, IValidator<NewsletterSubscribeRequest> validator)
        {
            _mediator = mediator;
            _validator = validator;
        }

        [HttpPost]
        public async Task<IActionResult> Subscribe([FromForm] NewsletterSubscribeRequest request)
        {
            var validationResult = await _validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                TempData["SubscriptionError"] = validationResult.Errors.First().ErrorMessage;
                return Redirect(Request.Headers["Referer"].ToString());
            }

            try
            {
                await _mediator.Send(request);
                TempData["SubscriptionSuccess"] = true;
            }
            catch (Exception ex)
            {
                TempData["SubscriptionError"] = ex.Message;
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
