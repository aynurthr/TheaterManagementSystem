using FluentValidation;
using Theater.Application.Modules.GenreModule.Commands.GenreEditCommand;

namespace Theater.Application.Validators.Genre
{
    public class GenreEditRequestValidator : AbstractValidator<GenreEditRequest>
    {
        public GenreEditRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");
        }
    }
}
