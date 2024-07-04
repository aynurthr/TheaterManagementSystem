using Theater.Domain.Models.Entities.Membership;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Theater.Application.Modules.AccountModule.Commands.SigninCommand
{
    class SigninRequestHandler : IRequestHandler<SigninRequest, SigninResult>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signinManager;

        public SigninRequestHandler(UserManager<AppUser> userManager, SignInManager<AppUser> signinManager)
        {
            this.userManager = userManager;
            this.signinManager = signinManager;
        }

        public async Task<SigninResult> Handle(SigninRequest request, CancellationToken cancellationToken)
        {
            AppUser user = null;

            if (request.UserName.Contains("@"))
            {
                user = await userManager.FindByEmailAsync(request.UserName);
            }
            else if (request.UserName.All(char.IsDigit))
            {
                user = await userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.UserName);
            }
            else
            {
                user = await userManager.FindByNameAsync(request.UserName);
            }

            if (user is null)
            {
                return new SigninResult { Succeeded = false, ErrorMessage = "Invalid email, username, or phone number." };
            }

            var isSuccess = await signinManager.CheckPasswordSignInAsync(user, request.Password, true);

            if (isSuccess.Succeeded)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                };

                var claimsIdentity = new ClaimsIdentity(claims, request.Scheme);

                return new SigninResult { Succeeded = true, Principal = new ClaimsPrincipal(claimsIdentity) };
            }
            else
            {
                return new SigninResult { Succeeded = false, ErrorMessage = "Invalid username/email/phone number or password." };
            }
        }
    }
}
