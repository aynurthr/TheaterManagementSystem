using Theater.Domain.Models.Entities.Membership;
using Theater.Infrastructure.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Theater.Application.Modules.AccountModule.Commands.SigninCommand
{
    class SigninRequestHandler : IRequestHandler<SigninRequest, ClaimsPrincipal>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signinManager;

        public SigninRequestHandler(UserManager<AppUser> userManager, SignInManager<AppUser> signinManager)
        {
            this.userManager = userManager;
            this.signinManager = signinManager;
        }


        public async Task<ClaimsPrincipal> Handle(SigninRequest request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.UserName);

            if (user is null)
                throw new UserNotFoundException();

            var isSuccess = await signinManager.CheckPasswordSignInAsync(user, request.Password, true);

            if (isSuccess.Succeeded)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                };

                var claimsIdentity = new ClaimsIdentity(claims, request.Scheme);

                return new ClaimsPrincipal(claimsIdentity);
            }
            else
                throw new UserNotFoundException();
        }
    }
}
