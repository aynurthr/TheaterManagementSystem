using FluentValidation;

namespace Theater.Application.Modules.PosterModule.Commands.PosterAddCommand
{
    public class PosterAddRequestValidator : AbstractValidator<PosterAddRequest>
    {
        public PosterAddRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

            RuleFor(x => x.Duration)
                .NotEmpty().WithMessage("Duration is required.")
                .MaximumLength(10).WithMessage("Duration cannot exceed 10 characters.");

            RuleFor(x => x.GenreId)
               .GreaterThan(0).WithMessage("Genre is required.");

            RuleFor(x => x.Age)
                .NotEmpty().WithMessage("Age is required.")
                .MaximumLength(3).WithMessage("Age cannot exceed 3 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.");

            RuleFor(x => x.Image)
                .NotNull().WithMessage("Image is required.");

            RuleForEach(x => x.Roles).SetValidator(new RoleDtoValidator());
            RuleForEach(x => x.ShowDates).SetValidator(new ShowDateDtoValidator());
        }

        public class RoleDtoValidator : AbstractValidator<RoleDto>
        {
            public RoleDtoValidator()
            {
                RuleFor(x => x.RoleName)
                    .NotEmpty().WithMessage("Role name is required.");
                RuleFor(x => x.ActorId)
                    .GreaterThan(0).WithMessage("Actor is required.");
            }
        }

        public class ShowDateDtoValidator : AbstractValidator<ShowDateDto>
        {
            public ShowDateDtoValidator()
            {
                RuleFor(x => x.Date)
                    .GreaterThan(DateTime.Now).WithMessage("Show date must be in the future.");
                RuleFor(x => x.HallId)
                    .GreaterThan(0).WithMessage("Hall is required.");
            }
        }
    }
}
