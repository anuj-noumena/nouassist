using FluentValidation;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Constants;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.RevenueModel.Dtos;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Validation
{
    public class CreateRevenueModelDtoValidator : AbstractValidator<CreateRevenueModelDto>
    {
        public CreateRevenueModelDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(BusinessModelManagementConsts.RevenueModel.MaxNameLength);

            RuleFor(x => x.Description)
                .MaximumLength(BusinessModelManagementConsts.RevenueModel.MaxDescriptionLength);

            RuleFor(x => x.Type)
                .IsInEnum()
                .WithMessage("Invalid revenue model type");
        }
    }

    public class CreateRevenueScenarioDtoValidator : AbstractValidator<CreateRevenueScenarioDto>
    {
        public CreateRevenueScenarioDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(BusinessModelManagementConsts.RevenueModel.MaxScenarioNameLength);

            RuleFor(x => x.Description)
                .MaximumLength(BusinessModelManagementConsts.RevenueModel.MaxScenarioDescriptionLength);

            RuleFor(x => x.Parameters)
                .NotNull()
                .SetValidator(new ScenarioParametersDtoValidator());
        }
    }

    public class ScenarioParametersDtoValidator : AbstractValidator<ScenarioParametersDto>
    {
        public ScenarioParametersDtoValidator()
        {
            RuleFor(x => x.CustomerCount)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Customer count must be non-negative");

            RuleFor(x => x.AverageRevenuePerUser)
                .GreaterThanOrEqualTo(0)
                .WithMessage("ARPU must be non-negative");

            RuleFor(x => x.ChurnRate)
                .InclusiveBetween(0.0, 1.0)
                .WithMessage("Churn rate must be between 0 and 1");

            RuleFor(x => x.GrowthRate)
                .GreaterThanOrEqualTo(-1.0)
                .WithMessage("Growth rate cannot be less than -100%");

            RuleFor(x => x.TimeHorizonMonths)
                .GreaterThan(0)
                .LessThanOrEqualTo(120)
                .WithMessage("Time horizon must be between 1 and 120 months");

            RuleFor(x => x.CostPerCustomer)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Cost per customer must be non-negative");
        }
    }
}
