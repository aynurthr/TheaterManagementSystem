using FluentValidation;

namespace Theater.Application.Modules.RoleModule.Commands.RoleEditCommand
{
    public class RoleEditRequestValidator : AbstractValidator<RoleEditRequest>
    {
        public RoleEditRequestValidator()
        {
            RuleFor(x => x.RoleName)
                .NotEmpty().WithMessage("Role Name is required.")
                .MinimumLength(3).WithMessage("Role Name must be at least 3 characters long.");

            RuleFor(x => x.ActorId)
                .NotEmpty().WithMessage("Actor is required.");

            RuleFor(x => x.PosterId)
                .NotEmpty().WithMessage("Poster is required.");
        }
    }
}
