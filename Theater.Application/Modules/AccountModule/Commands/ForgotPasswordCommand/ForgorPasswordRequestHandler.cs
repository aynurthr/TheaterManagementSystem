//using MediatR;
//using Microsoft.AspNetCore.Identity;
//using Theater.Domain.Models.Entities.Membership;
//using Theater.Infrastructure.Abstracts;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Theater.Application.Modules.AccountModule.Commands.ForgotPasswordCommand
//{
//    public class ForgotPasswordRequestHandler : IRequestHandler<ForgotPasswordRequest>
//    {
//        private readonly UserManager<AppUser> _userManager;
//        private readonly IEmailService _emailService;
//        private readonly ICryptoService _cryptoService;

//        public ForgotPasswordRequestHandler(UserManager<AppUser> userManager, IEmailService emailService, ICryptoService cryptoService)
//        {
//            _userManager = userManager;
//            _emailService = emailService;
//            _cryptoService = cryptoService;
//        }

//        public async Task Handle(ForgotPasswordRequest request, CancellationToken cancellationToken)
//        {
//            var user = await _userManager.FindByEmailAsync(request.Email);
//            if (user == null)
//            {
//                throw new Exception("User not found");
//            }

//            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
//            token = _cryptoService.Encrypt(token);
//            var callbackUrl = $"https://yourapp.com/reset-password?token={token}&email={request.Email}";

//            await _emailService.SendEmailAsync(
//                request.Email,
//                "Reset Password",
//                $"Please reset your password by clicking <a href='{callbackUrl}'>here</a>.");
//        }
//    }
//}
