using Theater.Domain.Models.Entities.Membership;
using Theater.Infrastructure.Abstracts;
using Theater.Infrastructure.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Web;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Theater.Application.Modules.AccountModule.Commands.ForgetPasswordCommand
{
    class ForgetPasswordRequestHandler : IRequestHandler<ForgetPasswordRequest>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly ICryptoService cryptoService;
        private readonly IEmailService emailService;
        private readonly IActionContextAccessor ctx;

        public ForgetPasswordRequestHandler(UserManager<AppUser> userManager, ICryptoService cryptoService, IEmailService emailService, IActionContextAccessor ctx)
        {
            this.userManager = userManager;
            this.cryptoService = cryptoService;
            this.emailService = emailService;
            this.ctx = ctx;
        }

        public async Task Handle(ForgetPasswordRequest request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                var errors = new Dictionary<string, IEnumerable<string>>
                {
                    ["Email"] = new[] { "Email not found. Please try again." }
                };
                throw new BadRequestException("Email not found", errors);
            }


            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            token = cryptoService.Encrypt($"{user.Email}-{token}");
            token = HttpUtility.UrlEncode(token);

            var resetUrl = $"{ctx.ActionContext.HttpContext.Request.Scheme}://{ctx.ActionContext.HttpContext.Request.Host}/reset-password.html?token={token}";

            await emailService.SendEmailAsync(request.Email, "Kazan Theater Password Reset", @$"
    <html>
    <body style='font-family: Arial, sans-serif; color: #333;'>
        <h2>Password Reset Request</h2>
        <p>Hello,</p>
        <p>You requested a password reset for your account. Click the link below to reset your password:</p>
        <p><a href='{resetUrl}' style='color: #1a73e8;'>Reset your password</a></p>
        <i>Best regards,<br>Kazan State Theater of The Young Spectators</i>
    </body>
    </html>
");
        }
    }
}
