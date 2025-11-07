using FluentValidation;
using NouAssist.CoreProductivity.ProjectManagement.SuccessCriteria.Dtos;

namespace NouAssist.CoreProductivity.ProjectManagement.SuccessCriteria.Validators
{
    public class CreateSuccessCriterionDtoValidator : AbstractValidator<CreateSuccessCriterionDto>
    {
        public CreateSuccessCriterionDtoValidator()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty();

            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(500);

            RuleFor(x => x.Description)
                .MaximumLength(2000)
                .When(x => !string.IsNullOrWhiteSpace(x.Description));

            RuleFor(x => x.CriterionType)
                .IsInEnum();

            RuleFor(x => x.Weight)
                .InclusiveBetween(0, 10);

            RuleFor(x => x.MeasurementMethod)
                .MaximumLength(500)
                .When(x => !string.IsNullOrWhiteSpace(x.MeasurementMethod));

            RuleFor(x => x.TargetValue)
                .MaximumLength(500)
                .When(x => !string.IsNullOrWhiteSpace(x.TargetValue));
        }
    }
}
