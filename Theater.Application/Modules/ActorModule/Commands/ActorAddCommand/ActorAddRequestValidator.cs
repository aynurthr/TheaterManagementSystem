using FluentValidation;

namespace Theater.Application.Modules.ActorModule.Commands.ActorAddCommand
{
    public class ActorAddRequestValidator : AbstractValidator<ActorAddRequest>
    {
        public ActorAddRequestValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required.")
                .MaximumLength(100).WithMessage("Full name cannot exceed 100 characters.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

            RuleFor(x => x.Image)
                .NotNull().WithMessage("Image is required.");
        }
    }
}
