using FluentValidation;

namespace NouAssist.TimeManagement.Meetings.Meetings.Dtos
{
    public class CreateMeetingDtoValidator : AbstractValidator<CreateMeetingDto>
    {
        public CreateMeetingDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Description)
                .MaximumLength(2000);

            RuleFor(x => x.StartAt)
                .NotEmpty()
                .GreaterThan(System.DateTime.UtcNow)
                .WithMessage("Meeting start time must be in the future");

            RuleFor(x => x.EndAt)
                .NotEmpty()
                .GreaterThan(x => x.StartAt)
                .WithMessage("Meeting end time must be after start time");

            RuleFor(x => x.MeetingType)
                .IsInEnum();

            RuleFor(x => x.MeetingUrl)
                .MaximumLength(1000);

            RuleForEach(x => x.Attendees)
                .SetValidator(new AddMeetingAttendeeDtoValidator())
                .When(x => x.Attendees != null);

            RuleForEach(x => x.AgendaItems)
                .SetValidator(new CreateAgendaItemDtoValidator())
                .When(x => x.AgendaItems != null);
        }
    }

    public class AddMeetingAttendeeDtoValidator : AbstractValidator<AddMeetingAttendeeDto>
    {
        public AddMeetingAttendeeDtoValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.Role)
                .IsInEnum();
        }
    }

    public class CreateAgendaItemDtoValidator : AbstractValidator<CreateAgendaItemDto>
    {
        public CreateAgendaItemDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.DurationMinutes)
                .GreaterThan(0)
                .When(x => x.DurationMinutes.HasValue);
        }
    }
}
