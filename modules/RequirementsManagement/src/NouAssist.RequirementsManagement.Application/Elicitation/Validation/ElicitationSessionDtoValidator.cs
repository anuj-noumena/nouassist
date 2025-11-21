using System;
using FluentValidation;
using NouAssist.RequirementsManagement.Elicitation.Constants;
using NouAssist.RequirementsManagement.Elicitation.ElicitationSessions.Dtos;

namespace NouAssist.RequirementsManagement.Elicitation.Validation
{
    public class CreateElicitationSessionDtoValidator : AbstractValidator<CreateElicitationSessionDto>
    {
        public CreateElicitationSessionDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(ElicitationConsts.MaxSessionTitleLength);

            RuleFor(x => x.Description)
                .MaximumLength(ElicitationConsts.MaxSessionDescriptionLength);

            RuleFor(x => x.OrganizerId)
                .NotEmpty().WithMessage("Organizer is required");

            RuleFor(x => x.Schedule)
                .NotNull().WithMessage("Schedule is required")
                .SetValidator(new CreateSessionScheduleDtoValidator());

            RuleFor(x => x.ParticipantIds)
                .NotNull().WithMessage("Participant list is required");
        }
    }

    public class UpdateElicitationSessionDtoValidator : AbstractValidator<UpdateElicitationSessionDto>
    {
        public UpdateElicitationSessionDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(ElicitationConsts.MaxSessionTitleLength);

            RuleFor(x => x.Description)
                .MaximumLength(ElicitationConsts.MaxSessionDescriptionLength);

            RuleFor(x => x.Schedule)
                .NotNull().WithMessage("Schedule is required")
                .SetValidator(new UpdateSessionScheduleDtoValidator());

            RuleFor(x => x.ParticipantIds)
                .NotNull().WithMessage("Participant list is required");
        }
    }

    public class CreateSessionScheduleDtoValidator : AbstractValidator<CreateSessionScheduleDto>
    {
        public CreateSessionScheduleDtoValidator()
        {
            RuleFor(x => x.StartTime)
                .NotEmpty().WithMessage("Start time is required")
                .Must((schedule, startTime) => startTime > DateTime.Now)
                .WithMessage("Start time must be in the future");

            RuleFor(x => x.EndTime)
                .NotEmpty().WithMessage("End time is required")
                .Must((schedule, endTime) => endTime > schedule.StartTime)
                .WithMessage("End time must be after start time");

            RuleFor(x => x.Location)
                .MaximumLength(ElicitationConsts.MaxLocationLength);

            RuleFor(x => x.TimeZone)
                .MaximumLength(50);
        }
    }

    public class UpdateSessionScheduleDtoValidator : AbstractValidator<UpdateSessionScheduleDto>
    {
        public UpdateSessionScheduleDtoValidator()
        {
            RuleFor(x => x.StartTime)
                .NotEmpty().WithMessage("Start time is required");

            RuleFor(x => x.EndTime)
                .NotEmpty().WithMessage("End time is required")
                .Must((schedule, endTime) => endTime > schedule.StartTime)
                .WithMessage("End time must be after start time");

            RuleFor(x => x.Location)
                .MaximumLength(ElicitationConsts.MaxLocationLength);

            RuleFor(x => x.TimeZone)
                .MaximumLength(50);
        }
    }
}
