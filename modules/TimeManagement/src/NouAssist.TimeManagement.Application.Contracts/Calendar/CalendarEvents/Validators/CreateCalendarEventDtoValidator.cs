using NouAssist.TimeManagement.Calendar.CalendarEvents.Dtos;

namespace NouAssist.TimeManagement.Calendar.CalendarEvents.Validators
{
    public class CreateCalendarEventDtoValidator : AbstractValidator<CreateCalendarEventDto>
    {
        public CreateCalendarEventDtoValidator()
        {
            RuleFor(x => x.CalendarId)
                .NotEmpty();

            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Description)
                .MaximumLength(2000);

            RuleFor(x => x.EventLocation)
                .MaximumLength(500);

            RuleFor(x => x.StartAt)
                .NotEmpty()
                .LessThan(x => x.EndAt)
                .WithMessage("Start time must be before end time");

            RuleFor(x => x.EndAt)
                .NotEmpty()
                .GreaterThan(x => x.StartAt)
                .WithMessage("End time must be after start time");

            When(x => x.IsAllDay, () =>
            {
                RuleFor(x => x.EndAt.Subtract(x.StartAt))
                    .Must(duration => duration.TotalDays >= 1)
                    .WithMessage("All-day events must be at least 1 day long");
            });

            When(x => !x.IsAllDay, () =>
            {
                RuleFor(x => x.EndAt.Subtract(x.StartAt))
                    .Must(duration => duration.TotalMinutes >= 1)
                    .WithMessage("Event duration must be at least 1 minute");
            });

            When(x => x.IsRecurring, () =>
            {
                RuleFor(x => x.RecurrenceRule)
                    .NotNull()
                    .WithMessage("Recurrence rule is required for recurring events");
            });
        }
    }
}
