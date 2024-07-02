using MediatR;
using System.Security.Claims;

namespace Theater.Application.Modules.AccountModule.Commands.PrincipalFillCommand
{
    public class PrincipalFillRequest : IRequest
    {
        public PrincipalFillRequest(ClaimsIdentity identity) => this.Identity = identity;

        internal ClaimsIdentity Identity { get; set; }
    }
}
