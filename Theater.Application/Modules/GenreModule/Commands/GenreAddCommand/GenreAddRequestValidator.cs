using FluentValidation;
using Theater.Application.Modules.GenreModule.Commands.GenreAddCommand;

namespace Theater.Application.Validators.Genre
{
    public class GenreAddRequestValidator : AbstractValidator<GenreAddRequest>
    {
        public GenreAddRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");
        }
    }
}
