using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Theater.Application.Modules.AccountModule.Commands.EmailConfirmationCommand;
using Theater.Application.Modules.AccountModule.Commands.SigninCommand;
using Theater.Application.Modules.AccountModule.Commands.SignupCommand;

namespace Theater.Presentation.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMediator mediator;

        public AccountController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [AllowAnonymous]
        [Route("/signin.html")]
        public async Task<IActionResult> Signin()
        {
            if (TempData["EmailConfirmationSent"] != null)
            {
                ViewBag.EmailConfirmationSent = TempData["EmailConfirmationSent"];
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("/signin.html")]
        public async Task<IActionResult> Signin(SigninRequest request)
        {
            request.Scheme = CookieAuthenticationDefaults.AuthenticationScheme;

            var result = await mediator.Send(request);

            if (result.Succeeded)
            {
                var props = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(20)
                };

                await HttpContext.SignInAsync(request.Scheme, result.Principal, props);

                var callbackUrl = Request.Query["ReturnUrl"];

                if (!string.IsNullOrWhiteSpace(callbackUrl))
                {
                    return Redirect(callbackUrl!);
                }

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = result.ErrorMessage;
                return View(request);
            }
        }

        [HttpGet]
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
            await mediator.Send(request);

            TempData["EmailConfirmationSent"] = "Confirmation email has been sent. Please check your email and confirm your account.";

            return RedirectToAction("Signin", "Account");
        }

        [AllowAnonymous]
        [Route("/email-confirmation.html")]
        public async Task<IActionResult> EmailConfirmation([FromQuery] EmailConfirmationRequest request)
        {
            await mediator.Send(request);

            TempData["EmailConfirmed"] = "Your email has been confirmed successfully. You can now log in.";

            return RedirectToAction("EmailConfirmed", "Account");
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("/email-confirmed.html")]
        public IActionResult EmailConfirmed()
        {
            if (TempData["EmailConfirmed"] != null)
            {
                ViewBag.EmailConfirmed = TempData["EmailConfirmed"];
            }
            return View();
        }

        [Route("/signout.html")]
        public async Task<IActionResult> Signout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Signin", "Account");
        }


        //[HttpGet]
        //[Route("/forgot-password")]
        //public IActionResult ForgotPassword()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[Route("/forgot-password")]
        //public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        await mediator.Send(request);
        //        ViewBag.Message = "Password reset link has been sent to your email.";
        //    }

        //    return View(request);
        //}



        [Route("/accessdenied.html")]
        public IActionResult Denied()
        {
            return View();
        }


    }
}
