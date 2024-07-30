using FluentValidation;

namespace Theater.Application.Modules.ContactPostModule.Commands.ContactPostReplyCommand
{
    public class ContactPostReplyRequestValidator : AbstractValidator<ContactPostReplyRequest>
    {
        public ContactPostReplyRequestValidator()
        {

            RuleFor(x => x.ReplyMessage)
                .NotEmpty().WithMessage("Message is required")
                .MaximumLength(500).WithMessage("Message cannot exceed 500 characters");
        }
    }
}
