using FluentValidation;
using NouAssist.GTD.Dtos;

namespace NouAssist.GTD.Validators
{
    public class CreateGtdHorizonDtoValidator : AbstractValidator<CreateGtdHorizonDto>
    {
        public CreateGtdHorizonDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(x => x.Description)
                .MaximumLength(5000)
                .When(x => !string.IsNullOrWhiteSpace(x.Description));
        }
    }

    public class CreateTicklerItemDtoValidator : AbstractValidator<CreateTicklerItemDto>
    {
        public CreateTicklerItemDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(x => x.TickleDate)
                .NotEmpty()
                .GreaterThanOrEqualTo(DateTime.Today)
                .WithMessage("Tickle date cannot be in the past.");

            RuleFor(x => x.ImportanceScore)
                .InclusiveBetween(0.0, 1.0)
                .When(x => x.ImportanceScore.HasValue);

            RuleFor(x => x.UrgencyScore)
                .InclusiveBetween(0.0, 1.0)
                .When(x => x.UrgencyScore.HasValue);
        }
    }

    public class SetRecurrenceDtoValidator : AbstractValidator<SetRecurrenceDto>
    {
        public SetRecurrenceDtoValidator()
        {
            RuleFor(x => x.Frequency)
                .NotEmpty()
                .Must(f => new[] { "Daily", "Weekly", "Monthly", "Yearly" }.Contains(f))
                .WithMessage("Frequency must be Daily, Weekly, Monthly, or Yearly.");

            RuleFor(x => x.Interval)
                .GreaterThan(0)
                .WithMessage("Interval must be greater than 0.");

            RuleFor(x => x)
                .Must(x => !(x.Until.HasValue && x.Count.HasValue))
                .WithMessage("Cannot specify both Until and Count.");
        }
    }

    public class CreateSomedayMaybeDtoValidator : AbstractValidator<CreateSomedayMaybeItemDto>
    {
        public CreateSomedayMaybeDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(x => x.Description)
                .MaximumLength(5000)
                .When(x => !string.IsNullOrWhiteSpace(x.Description));
        }
    }

    public class ReviewSomedayMaybeDtoValidator : AbstractValidator<ReviewSomedayMaybeDto>
    {
        public ReviewSomedayMaybeDtoValidator()
        {
            RuleFor(x => x.InterestLevel)
                .InclusiveBetween(1, 5)
                .When(x => x.InterestLevel.HasValue)
                .WithMessage("Interest level must be between 1 and 5.");
        }
    }
}
