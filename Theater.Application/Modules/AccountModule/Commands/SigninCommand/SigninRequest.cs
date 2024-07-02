using MediatR;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace Theater.Application.Modules.AccountModule.Commands.SigninCommand
{
    public class SigninRequest : IRequest<ClaimsPrincipal>
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        [JsonIgnore]
        public string Scheme { get; set; }
    }
}
