using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Theater.Domain.Models.Entities.Membership;
using Theater.Infrastructure.Abstracts;
using Theater.Infrastructure.Exceptions;

namespace Theater.Application.Modules.AccountModule.Commands.ResetPasswordCommand
{
    public class ResetPasswordRequestHandler : IRequestHandler<ResetPasswordRequest>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ICryptoService _cryptoService;

        public ResetPasswordRequestHandler(UserManager<AppUser> userManager, ICryptoService cryptoService)
        {
            _userManager = userManager;
            _cryptoService = cryptoService;
        }

        public async Task Handle(ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            string text = _cryptoService.Decrypt(request.Token);

            var match = Regex.Match(text, @"^(?<email>([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?))-(?<identityToken>.+)$");

            if (!match.Success)
            {
                var errors = new Dictionary<string, IEnumerable<string>>
                {
                    [nameof(request.Token)] = new[] { "Bad token" }
                };

                throw new BadRequestException("Bad token request", errors);
            }

            var user = await _userManager.FindByEmailAsync(match.Groups["email"].Value);

            if (user == null)
            {
                var errors = new Dictionary<string, IEnumerable<string>>
                {
                    [nameof(request.Email)] = new[] { "User not found" }
                };

                throw new BadRequestException("User not found", errors);
            }

            if (await IsSameAsOldPassword(user, request.NewPassword))
            {
                var errors = new Dictionary<string, IEnumerable<string>>
                {
                    [nameof(request.NewPassword)] = new[] { "New password cannot be the same as the old password." }
                };

                throw new BadRequestException("New password is the same as the old password", errors);
            }

            var result = await _userManager.ResetPasswordAsync(user, match.Groups["identityToken"].Value, request.NewPassword);

            if (!result.Succeeded)
            {
                var errors = result.Errors.ToDictionary(k => k.Code, v => (IEnumerable<string>)new[] { v.Description });

                throw new BadRequestException("One or more errors occurred!", errors);
            }
        }

        private async Task<bool> IsSameAsOldPassword(AppUser user, string newPassword)
        {
            return await _userManager.CheckPasswordAsync(user, newPassword);
        }
    }
}
