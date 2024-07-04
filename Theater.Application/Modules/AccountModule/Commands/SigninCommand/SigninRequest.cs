using MediatR;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace Theater.Application.Modules.AccountModule.Commands.SigninCommand
{
    public class SigninRequest : IRequest<SigninResult>
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        [JsonIgnore]
        public string Scheme { get; set; }
    }
    public class SigninResult
    {
        public bool Succeeded { get; set; }
        public ClaimsPrincipal Principal { get; set; }
        public string ErrorMessage { get; set; }
    }
}
