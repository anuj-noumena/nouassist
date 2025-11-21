using FluentValidation;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.FinancialCalculations.Dtos;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.FinancialModels.Dtos;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.MetricCategories.Dtos;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.MetricDefinitions.Dtos;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.TrackedMetrics.Dtos;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.TrendAnalysis.Dtos;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Validation.FinancialCalculation;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Validation.FinancialModel;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Validation.MetricCategory;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Validation.MetricDefinition;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Validation.TrackedMetric;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Validation.TrendAnalysis;
using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Validation.TrendAnalysis
{
    public class ForecastMetricDtoValidator : AbstractValidator<ForecastMetricDto>
    {
        public ForecastMetricDtoValidator()
        {
            RuleFor(x => x.ForecastPeriods)
                .GreaterThan(0)
                .WithMessage("Forecast periods must be greater than 0")
                .LessThanOrEqualTo(36)
                .WithMessage("Forecast periods cannot exceed 36");

            RuleFor(x => x.Method)
                .MaximumLength(50)
                .When(x => !string.IsNullOrEmpty(x.Method))
                .WithMessage("Method name cannot exceed 50 characters");
        }
    }
}

// Final Update: src/NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Application/PerformanceFinancialModelingApplicationModule.cs
[DependsOn(
    typeof(PerformanceFinancialModelingDomainModule),
    typeof(PerformanceFinancialModelingApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpFluentValidationModule)
)]
public class PerformanceFinancialModelingApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // Register AutoMapper profile
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<PerformanceFinancialModelingApplicationModule>();
            options.ValidateProfile = true;
        });

        // Register FluentValidation validators
        context.Services.AddTransient<IValidator<CreateTrackedMetricDto>, CreateTrackedMetricDtoValidator>();
        context.Services.AddTransient<IValidator<CreateFinancialModelDto>, CreateFinancialModelDtoValidator>();
        context.Services.AddTransient<IValidator<CreateMetricDefinitionDto>, CreateMetricDefinitionDtoValidator>();
        context.Services.AddTransient<IValidator<CreateMetricCategoryDto>, CreateMetricCategoryDtoValidator>();
        context.Services.AddTransient<IValidator<PerformSensitivityAnalysisDto>, PerformSensitivityAnalysisDtoValidator>();
        context.Services.AddTransient<IValidator<ForecastMetricDto>, ForecastMetricDtoValidator>();

        // Application services are automatically registered by convention based on interface naming
        // All I*AppService interfaces with their implementations are auto-registered
    }
}
