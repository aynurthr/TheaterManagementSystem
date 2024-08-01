using MediatR;

namespace Theater.Application.Modules.AccountModule.Commands.ForgetPasswordCommand
{
    public class ForgetPasswordRequest : IRequest
    {
        public string Email { get; set; }
    }
}
