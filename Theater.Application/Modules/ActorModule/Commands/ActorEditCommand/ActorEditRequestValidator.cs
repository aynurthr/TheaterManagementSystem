


using FluentValidation;

namespace Theater.Application.Modules.ActorModule.Commands.ActorEditCommand
{
    public class ActorEditRequestValidator : AbstractValidator<ActorEditRequest>
    {
        public ActorEditRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required.")
                .MaximumLength(100).WithMessage("Full name cannot exceed 100 characters.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

            RuleFor(x => x.Image)
                .NotNull().WithMessage("Image is required.")
                .When(x => x.Image != null);
        }
    }
}