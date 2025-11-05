using FluentValidation;
using NouAssist.Core.Dtos;

namespace NouAssist.Core.Validators
{
    public class CreateTagDtoValidator : AbstractValidator<CreateTagDto>
    {
        public CreateTagDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(50);
        }
    }

    public class CreateContextDtoValidator : AbstractValidator<CreateContextDto>
    {
        public CreateContextDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(x => x.EnergyLevel)
                .InclusiveBetween(1, 5)
                .When(x => x.EnergyLevel.HasValue)
                .WithMessage("Energy level must be between 1 and 5.");
        }
    }

    public class CreateAreaDtoValidator : AbstractValidator<CreateAreaDto>
    {
        public CreateAreaDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(255);
        }
    }

    public class CreateInboxItemDtoValidator : AbstractValidator<CreateInboxItemDto>
    {
        public CreateInboxItemDtoValidator()
        {
            RuleFor(x => x.InboxContent)
                .NotEmpty()
                .MaximumLength(5000);
        }
    }
}
