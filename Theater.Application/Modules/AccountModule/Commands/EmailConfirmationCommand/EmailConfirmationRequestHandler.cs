using Theater.Domain.Models.Entities.Membership;
using Theater.Infrastructure.Abstracts;
using Theater.Infrastructure.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace Theater.Application.Modules.AccountModule.Commands.EmailConfirmationCommand
{
    class EmailConfirmationRequestHandler : IRequestHandler<EmailConfirmationRequest>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly ICryptoService cryptoService;

        public EmailConfirmationRequestHandler(UserManager<AppUser> userManager, ICryptoService cryptoService)
        {
            this.userManager = userManager;
            this.cryptoService = cryptoService;
        }

        public async Task Handle(EmailConfirmationRequest request, CancellationToken cancellationToken)
        {
            string text = cryptoService.Decrypt(request.Token);

            var match = Regex.Match(text, @"^(?<email>([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?))-(?<identityToken>.+)$");

            if (!match.Success)
            {
                var errors = new Dictionary<string, IEnumerable<string>>
                {
                    [nameof(request.Token)] = new[] { "Bad token" }
                };

                throw new BadRequestException("Bad token request", errors);
            }



            var user = await userManager.FindByEmailAsync(match.Groups["email"].Value);

            await userManager.ConfirmEmailAsync(user, match.Groups["identityToken"].Value);
        }
    }
}
