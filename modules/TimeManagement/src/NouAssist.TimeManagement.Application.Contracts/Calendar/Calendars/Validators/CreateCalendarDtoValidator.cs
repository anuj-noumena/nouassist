using FluentValidation;
using NouAssist.TimeManagement.Calendar.Calendars.Dtos;

namespace NouAssist.TimeManagement.Calendar.Calendars.Validators
{
    public class CreateCalendarDtoValidator : AbstractValidator<CreateCalendarDto>
    {
        public CreateCalendarDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Description)
                .MaximumLength(2000);

            RuleFor(x => x.Color)
                .MaximumLength(50);

            RuleFor(x => x.Timezone)
                .MaximumLength(100);
        }
    }
}
