using MediatR;

namespace Theater.Application.Modules.AccountModule.Commands.ResetPasswordCommand
{
    public class ResetPasswordRequest : IRequest
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

    }
}
