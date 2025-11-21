using FluentValidation;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Constants;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Dtos.Base;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.MetricDefinitions.Dtos;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Validation.MetricDefinition
{
    public class CreateMetricDefinitionDtoValidator : AbstractValidator<CreateMetricDefinitionDto>
    {
        public CreateMetricDefinitionDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Definition name is required")
                .MaximumLength(PerformanceFinancialModelingConsts.MaxDefinitionNameLength)
                .WithMessage($"Definition name cannot exceed {PerformanceFinancialModelingConsts.MaxDefinitionNameLength} characters");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Definition description is required")
                .MaximumLength(PerformanceFinancialModelingConsts.MaxDefinitionDescriptionLength)
                .WithMessage($"Definition description cannot exceed {PerformanceFinancialModelingConsts.MaxDefinitionDescriptionLength} characters");

            RuleFor(x => x.Type)
                .IsInEnum()
                .WithMessage("Invalid metric type");

            RuleFor(x => x.DataType)
                .IsInEnum()
                .WithMessage("Invalid data type");

            RuleFor(x => x.Unit)
                .MaximumLength(PerformanceFinancialModelingConsts.MaxMetricUnitLength)
                .When(x => !string.IsNullOrEmpty(x.Unit))
                .WithMessage($"Unit cannot exceed {PerformanceFinancialModelingConsts.MaxMetricUnitLength} characters");

            RuleFor(x => x.CalculationRule)
                .NotNull()
                .WithMessage("Calculation rule is required")
                .SetValidator(new CalculationRuleDtoValidator());
        }
    }

    public class CalculationRuleDtoValidator : AbstractValidator<CalculationRuleDto>
    {
        public CalculationRuleDtoValidator()
        {
            RuleFor(x => x.Formula)
                .NotEmpty()
                .WithMessage("Formula is required")
                .MaximumLength(PerformanceFinancialModelingConsts.MaxCalculationRuleLength)
                .WithMessage($"Formula cannot exceed {PerformanceFinancialModelingConsts.MaxCalculationRuleLength} characters");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Formula description is required")
                .MaximumLength(PerformanceFinancialModelingConsts.MaxDefinitionDescriptionLength)
                .WithMessage($"Formula description cannot exceed {PerformanceFinancialModelingConsts.MaxDefinitionDescriptionLength} characters");
        }
    }
}
