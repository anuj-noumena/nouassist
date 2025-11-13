using FluentValidation;

namespace NouAssist.FinancialManagement.FinancialPlanning.Dtos
{
    public class CreateBudgetDtoValidator : AbstractValidator<CreateBudgetDto>
    {
        public CreateBudgetDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Budget name is required")
                .MaximumLength(200);

            RuleFor(x => x.Type)
                .IsInEnum()
                .WithMessage("Valid budget type is required");

            RuleFor(x => x.PeriodStart)
                .NotEmpty()
                .WithMessage("Period start date is required");

            RuleFor(x => x.PeriodEnd)
                .NotEmpty()
                .WithMessage("Period end date is required")
                .GreaterThan(x => x.PeriodStart)
                .WithMessage("Period end must be after period start");

            RuleFor(x => x.Allocations)
                .NotEmpty()
                .WithMessage("At least one category allocation is required");

            RuleForEach(x => x.Allocations)
                .Must(a => a.BudgetedAmount > 0)
                .WithMessage("Budgeted amount must be positive");
        }
    }
}
