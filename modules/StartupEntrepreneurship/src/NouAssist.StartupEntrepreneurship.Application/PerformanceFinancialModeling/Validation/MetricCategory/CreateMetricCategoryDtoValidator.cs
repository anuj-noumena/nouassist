using FluentValidation;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Constants;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.MetricCategories.Dtos;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Validation.MetricCategory
{
    public class CreateMetricCategoryDtoValidator : AbstractValidator<CreateMetricCategoryDto>
    {
        public CreateMetricCategoryDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Category name is required")
                .MaximumLength(PerformanceFinancialModelingConsts.MaxCategoryNameLength)
                .WithMessage($"Category name cannot exceed {PerformanceFinancialModelingConsts.MaxCategoryNameLength} characters");

            RuleFor(x => x.Description)
                .MaximumLength(PerformanceFinancialModelingConsts.MaxCategoryDescriptionLength)
                .When(x => !string.IsNullOrEmpty(x.Description))
                .WithMessage($"Category description cannot exceed {PerformanceFinancialModelingConsts.MaxCategoryDescriptionLength} characters");

            RuleFor(x => x.Color)
                .MaximumLength(7)
                .When(x => !string.IsNullOrEmpty(x.Color))
                .WithMessage("Color code cannot exceed 7 characters")
                .Matches(@"^#[0-9A-Fa-f]{6}$")
                .When(x => !string.IsNullOrEmpty(x.Color))
                .WithMessage("Color must be a valid hex color code (e.g., #FF0000)");

            RuleFor(x => x.DisplayOrder)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Display order cannot be negative");
        }
    }
}
