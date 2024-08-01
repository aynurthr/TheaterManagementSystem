using FluentValidation;
using Theater.Application.Modules.AccountModule.Commands.ResetPasswordCommand;

public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordRequestValidator()
    {

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("NewPassword is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Confirm Password is required.")
            .Equal(x => x.NewPassword).WithMessage("Passwords do not match.");
    }
}
