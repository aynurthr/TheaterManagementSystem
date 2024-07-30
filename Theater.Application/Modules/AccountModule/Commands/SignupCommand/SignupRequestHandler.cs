using FluentValidation;
using FluentValidation.Results;
using Theater.Domain.Models.Entities.Membership;
using Theater.Infrastructure.Abstracts;
using Theater.Infrastructure.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Text.RegularExpressions;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Theater.Application.Modules.AccountModule.Commands.SignupCommand
{
    class SignupRequestHandler : IRequestHandler<SignupRequest>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IActionContextAccessor ctx;
        private readonly IEmailService emailService;
        private readonly ICryptoService cryptoService;
        private readonly IValidator<SignupRequest> validator;

        public SignupRequestHandler(UserManager<AppUser> userManager, IActionContextAccessor ctx, IEmailService emailService, ICryptoService cryptoService, IValidator<SignupRequest> validator)
        {
            this.userManager = userManager;
            this.ctx = ctx;
            this.emailService = emailService;
            this.cryptoService = cryptoService;
            this.validator = validator;
        }

        public async Task Handle(SignupRequest request, CancellationToken cancellationToken)
        {
            ValidationResult validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).AsEnumerable());

                throw new BadRequestException("Validation failed", errors);
            }

            var user = await userManager.FindByEmailAsync(request.Email);

            if (user is not null)
            {
                var errors = new Dictionary<string, IEnumerable<string>>
                {
                    [nameof(request.Email)] = new[] { $"{request.Email} already taken" }
                };

                throw new BadRequestException("Email already taken", errors);
            }

            if (!string.IsNullOrEmpty(request.PhoneNumber))
            {
                var phoneNumberUser = await userManager.Users.SingleOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);
                if (phoneNumberUser != null)
                {
                    var errors = new Dictionary<string, IEnumerable<string>>
                    {
                        [nameof(request.PhoneNumber)] = new[] { "Phone number already taken" }
                    };

                    throw new BadRequestException("Phone number already taken", errors);
                }
            }

            var baseUserName = $"{request.Name}.{request.Surname}".ToLower();
            var sameUserName = await userManager.FindByNameAsync(baseUserName);

            if (sameUserName is not null)
            {
                var maxCount = userManager.Users.Count(m => m.UserName.StartsWith(baseUserName));
                baseUserName = $"{baseUserName}{maxCount + 1}".ToLower();
            }

            user = new AppUser
            {
                Name = request.Name,
                Surname = request.Surname,
                Email = request.Email,
                EmailConfirmed = false,
                UserName = baseUserName,
                PhoneNumber = request.PhoneNumber
            };

            var result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.ToDictionary(k => k.Code, v => (IEnumerable<string>)new[] { v.Description });

                throw new BadRequestException("One or more errors occurred!", errors);
            }

            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

            token = cryptoService.Encrypt($"{user.Email}-{token}");
            token = HttpUtility.UrlEncode(token);

            var confirmationUrl = $"{ctx.ActionContext.HttpContext.Request.Scheme}://{ctx.ActionContext.HttpContext.Request.Host}/email-confirmation.html?token={token}";

            await emailService.SendEmailAsync(request.Email, "Kazan Theater Registration", @$"
    <html>
    <body style='font-family: Arial, sans-serif; color: #333;'>
        <h2>Welcome to Kazan State Theater of The Young Spectators</h2>
        <p>Hello, Dear Customer,</p>
        <p>Thank you for registering with Kazan State Theater of The Young Spectators. To complete your registration, please confirm your email by clicking the link below:</p>
        <p><a href='{confirmationUrl}' style='color: #1a73e8;'>Confirm your email</a></p>
       <i>Best regards,<br>Kazan State Theater of The Young Spectators</i>
    </body>
    </html>
");
        }
    }
}
