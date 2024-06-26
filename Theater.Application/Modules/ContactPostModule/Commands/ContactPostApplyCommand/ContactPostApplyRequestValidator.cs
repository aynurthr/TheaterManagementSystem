using FluentValidation;

namespace Theater.Application.Modules.ContactPostModule.Commands.ContactPostApplyCommand
{
    public class ContactPostApplyRequestValidator : AbstractValidator<ContactPostApplyRequest>
    {
        public ContactPostApplyRequestValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full Name is required")
                .MaximumLength(100).WithMessage("Full Name cannot exceed 100 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("A valid email address is required");

            RuleFor(x => x.Subject)
                .NotEmpty().WithMessage("Subject is required")
                .MaximumLength(100).WithMessage("Subject cannot exceed 100 characters");

            RuleFor(x => x.Message)
                .NotEmpty().WithMessage("Message is required")
                .MaximumLength(1000).WithMessage("Message cannot exceed 1000 characters");
        }
    }
}
