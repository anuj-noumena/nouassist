using FluentValidation;
using NouAssist.CoreProductivity.TimeManagement.Timeblocks.Dtos;

namespace NouAssist.CoreProductivity.TimeManagement.Timeblocks.Validators
{
    public class CreateTimeblockDtoValidator : AbstractValidator<CreateTimeblockDto>
    {
        public CreateTimeblockDtoValidator()
        {
            RuleFor(x => x.CalendarEventId)
                .NotEmpty();

            RuleFor(x => x.Title)
                .MaximumLength(200);

            RuleFor(x => x.Description)
                .MaximumLength(2000);

            RuleFor(x => x.StartAt)
                .NotEmpty()
                .LessThan(x => x.EndAt);

            RuleFor(x => x.EndAt)
                .NotEmpty()
                .GreaterThan(x => x.StartAt);

            RuleFor(x => x.EndAt.Subtract(x.StartAt))
                .Must(duration => duration.TotalMinutes >= 15)
                .WithMessage("Timeblock duration must be at least 15 minutes");

            RuleFor(x => x.EndAt.Subtract(x.StartAt))
                .Must(duration => duration.TotalHours <= 8)
                .WithMessage("Timeblock duration should not exceed 8 hours");
        }
    }
}
