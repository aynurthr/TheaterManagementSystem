using FluentValidation;
using Theater.Application.Modules.PosterModule.Commands.PosterEditCommand;

namespace Theater.Application.Modules.PosterModule.Validators
{
    public class PosterEditRequestValidator : AbstractValidator<PosterEditRequest>
    {
        public PosterEditRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

            RuleFor(x => x.GenreId)
                .NotEmpty().WithMessage("Genre is required.");

            RuleFor(x => x.Duration)
                .NotEmpty().WithMessage("Duration is required.")
                .MaximumLength(50).WithMessage("Duration must not exceed 50 characters.");

            RuleFor(x => x.Age)
                .NotEmpty().WithMessage("Age is required.")
                .MaximumLength(10).WithMessage("Age must not exceed 10 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.");

            RuleFor(x => x.ShowDates)
                .NotEmpty().WithMessage("At least one show date is required.");

            RuleForEach(x => x.ShowDates).ChildRules(showDates =>
            {
                showDates.RuleFor(x => x.Date)
                    .NotEmpty().WithMessage("Show date is required.");
            });
        }
    }
}
