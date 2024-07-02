using MediatR;

namespace Theater.Application.Modules.AccountModule.Commands.SignupCommand
{
    public class SignupRequest : IRequest
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
