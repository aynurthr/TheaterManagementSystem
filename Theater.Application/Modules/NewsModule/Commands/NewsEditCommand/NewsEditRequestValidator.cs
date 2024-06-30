using FluentValidation;
using Theater.Application.Modules.NewsModule.Commands.NewsEditCommand;

namespace Theater.Application.Validators.News
{
    public class NewsEditRequestValidator : AbstractValidator<NewsEditRequest>
    {
        public NewsEditRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Date is required.");

            RuleFor(x => x.Image)
                .NotNull().WithMessage("Image is required.")
                .When(x => x.Image != null);
        }
    }
}
