using FluentValidation;

namespace Theater.Application.Modules.TeamMemberModule.Commands.TeamMemberAddCommand
{
    public class TeamMemberAddRequestValidator : AbstractValidator<TeamMemberAddRequest>
    {
        public TeamMemberAddRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required.")
                .MaximumLength(100).WithMessage("Role cannot exceed 100 characters.");

            RuleFor(x => x.Biography)
                .NotEmpty().WithMessage("Biography is required.");

            RuleFor(x => x.Image)
               .NotNull().WithMessage("Image is required.");
        }
    }
}
