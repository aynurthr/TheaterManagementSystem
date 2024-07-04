using Theater.Domain.Models.Entities.Membership;
using Theater.Infrastructure.Abstracts;
using Theater.Infrastructure.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Text.RegularExpressions;
using System.Web;

namespace Theater.Application.Modules.AccountModule.Commands.SignupCommand
{
    class SignupRequestHandler : IRequestHandler<SignupRequest>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IActionContextAccessor ctx;
        private readonly IEmailService emailService;
        private readonly ICryptoService cryptoService;

        public SignupRequestHandler(UserManager<AppUser> userManager, IActionContextAccessor ctx, IEmailService emailService, ICryptoService cryptoService)
        {
            this.userManager = userManager;
            this.ctx = ctx;
            this.emailService = emailService;
            this.cryptoService = cryptoService;
        }

        public async Task Handle(SignupRequest request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Email);


            if (user is not null)
            {
                var errors = new Dictionary<string, IEnumerable<string>>
                {
                    [nameof(request.Email)] = new[] { $"{request.Email} already taken" }
                };

                throw new BadRequestException("Email already taken", errors);
            }

            user = new AppUser
            {
                Name = request.Name,
                Surname = request.Surname,
                Email = request.Email,
                EmailConfirmed = false,
                UserName = $"{request.Name}.{request.Surname}".ToLower()
            };

            var sameUserName = await userManager.FindByNameAsync(user.UserName);

            if (sameUserName is not null)
            {
                var maxCount = userManager.Users.Count(m => m.UserName.StartsWith(user.UserName));



                user.UserName = $"{request.Name}.{request.Surname}{maxCount + 1}".ToLower();
            }

            var result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.ToDictionary(k => k.Code, v => (IEnumerable<string>)new[] { v.Description });

                throw new BadRequestException("One or more error occured!", errors);
            }

            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

            token = cryptoService.Encrypt($"{user.Email}-{token}");
            token = HttpUtility.UrlEncode(token);

            var confirmationUrl = $"{ctx.ActionContext.HttpContext.Request.Scheme}://{ctx.ActionContext.HttpContext.Request.Host}/email-confirmation.html?token={token}";

            await emailService.SendEmailAsync(request.Email, "Kazan Theater Registration", @$"Hello, Dear Customer.<br/>Please <a href='{confirmationUrl}'>Confirm</a> your email");
        }
    }
}
