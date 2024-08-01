using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Theater.Application.Modules.AccountModule.Commands.EmailConfirmationCommand;
using Theater.Application.Modules.AccountModule.Commands.ForgetPasswordCommand;
using Theater.Application.Modules.AccountModule.Commands.ResetPasswordCommand;
using Theater.Application.Modules.AccountModule.Commands.SigninCommand;
using Theater.Application.Modules.AccountModule.Commands.SignupCommand;
using Theater.Infrastructure.Exceptions;


namespace Theater.Presentation.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMediator mediator;
        private readonly IValidator<Application.Modules.AccountModule.Commands.ResetPasswordCommand.ResetPasswordRequest> _resetPasswordValidator;
        private readonly IValidator<ForgetPasswordRequest> _forgetPasswordValidator;

        public AccountController(IMediator mediator, IValidator<Application.Modules.AccountModule.Commands.ResetPasswordCommand.ResetPasswordRequest> resetPasswordValidator, IValidator<ForgetPasswordRequest> forgetPasswordValidator)
        {
            this.mediator = mediator;
            _resetPasswordValidator = resetPasswordValidator;
            _forgetPasswordValidator = forgetPasswordValidator;
        }

        [AllowAnonymous]
        [Route("/signin.html")]
        public async Task<IActionResult> Signin()
        {
            if (TempData["EmailConfirmationSent"] != null)
            {
                ViewBag.EmailConfirmationSent = TempData["EmailConfirmationSent"];
            }

            if (TempData["ResetPasswordSuccess"] != null)
            {
                ViewBag.ResetPasswordSuccess = TempData["ResetPasswordSuccess"];
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

        [Route("/accessdenied.html")]
        public IActionResult Denied()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("/forgot-password.html")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("/forgot-password.html")]
        public async Task<IActionResult> ForgotPassword(ForgetPasswordRequest request)
        {
            var validationResult = await _forgetPasswordValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return View(request);
            }

            try
            {
                await mediator.Send(request);
                ViewBag.ForgotPasswordSuccess = "Reset password email has been sent successfully.";
            }
            catch (BadRequestException ex)
            {
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(error.Key, string.Join(", ", error.Value));
                }
                return View(request);
            }

            return View();
        }


        [HttpGet]
        [AllowAnonymous]
        [Route("/reset-password.html")]
        public IActionResult ResetPassword(string token, string email)
        {
            var model = new Application.Modules.AccountModule.Commands.ResetPasswordCommand.ResetPasswordRequest { Token = token, Email = email };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("/reset-password.html")]
        public async Task<IActionResult> ResetPassword(Application.Modules.AccountModule.Commands.ResetPasswordCommand.ResetPasswordRequest request)
        {
            var validationResult = await _resetPasswordValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return View(request);
            }

            await mediator.Send(request);
            TempData["ResetPasswordSuccess"] = "Your password has been reset successfully.";
            return RedirectToAction("Signin");
        }



    }
}
