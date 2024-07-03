using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Theater.Application.Modules.AccountModule.Commands.EmailConfirmationCommand;
using Theater.Application.Modules.AccountModule.Commands.SigninCommand;
using Theater.Application.Modules.AccountModule.Commands.SignupCommand;

namespace Theater.Presentation.Controllers
{
    public class AccountController(IMediator mediator) : Controller
    {

        [AllowAnonymous]
        [Route("/signin.html")]
        public async Task<IActionResult> Signin()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("/signin.html")]
        public async Task<IActionResult> Signin(SigninRequest request)
        {
            request.Scheme = CookieAuthenticationDefaults.AuthenticationScheme;

            var principal = await mediator.Send(request);

            var props = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(20)
            };

            await HttpContext.SignInAsync(request.Scheme, principal, props);

            //ReturnUrl

            var callbackUrl = Request.Query["ReturnUrl"];

            if (!string.IsNullOrWhiteSpace(callbackUrl))
            {
                return Redirect(callbackUrl!);
            }

            return RedirectToAction("Index", controllerName: "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("/signup.html")]
        public async Task<IActionResult> Signup()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("/signup.html")]
        public async Task<IActionResult> Signup(SignupRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            await mediator.Send(request);

            return View("Signup");
        }

        [AllowAnonymous]
        [Route("/email-confirmation.html")]
        public async Task<IActionResult> EmailConfirmation([FromQuery] EmailConfirmationRequest request)
        {
            await mediator.Send(request);

            return Empty;
        }


        [Route("/accessdenied.html")]
        public IActionResult Denied()
        {
            return Content("bura icazeniz yoxdur");
        }

        [Route("/signout.html")]
        public IActionResult Signout()
        {
            return Content("Cixish burdandir");
        }
    }
}
