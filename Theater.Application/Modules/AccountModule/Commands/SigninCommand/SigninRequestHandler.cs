using Theater.Domain.Models.Entities.Membership;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

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
            var user = await userManager.FindByEmailAsync(request.UserName);

            if (user is null)
            {
                return new SigninResult { Succeeded = false, ErrorMessage = "Invalid email or password." };
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
                return new SigninResult { Succeeded = false, ErrorMessage = "Invalid email or password." };
            }
        }
    }

   
}
