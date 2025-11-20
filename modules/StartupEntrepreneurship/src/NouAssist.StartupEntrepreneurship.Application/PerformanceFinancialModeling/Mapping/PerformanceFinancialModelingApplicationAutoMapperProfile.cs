using AutoMapper;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Aggregates;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Dtos.Base;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Entities;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.FinancialModels.Dtos;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.MetricCategories.Dtos;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.MetricDefinitions.Dtos;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.TrackedMetrics.Dtos;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.ValueObjects;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Mapping
{
    public class PerformanceFinancialModelingApplicationAutoMapperProfile : Profile
    {
        public PerformanceFinancialModelingApplicationAutoMapperProfile()
        {
            // Value Object mappings
            CreateMap<MetricValue, MetricValueDto>().ReverseMap();
            CreateMap<CalculationRule, CalculationRuleDto>().ReverseMap();
            CreateMap<GoalTarget, GoalTargetDto>().ReverseMap();
            CreateMap<AssumptionValue, AssumptionValueDto>().ReverseMap();
            CreateMap<ProjectedAmount, ProjectedAmountDto>().ReverseMap();
            CreateMap<BurnRateValue, BurnRateValueDto>().ReverseMap();
            CreateMap<RunwayLength, RunwayLengthDto>().ReverseMap();
            CreateMap<BreakEvenResult, BreakEvenResultDto>().ReverseMap();

            // Value Object creation mappings (using factory methods)
            CreateMap<MetricValueDto, MetricValue>()
                .ConstructUsing(dto => new MetricValue(dto.Value, dto.DataType, dto.Unit))
                .ForAllMembers(opts => opts.Ignore());

            CreateMap<CalculationRuleDto, CalculationRule>()
                .ConstructUsing(dto => new CalculationRule(dto.Formula, dto.Description, dto.IsAutomated, dto.RequiredMetrics))
                .ForAllMembers(opts => opts.Ignore());

            CreateMap<GoalTargetDto, GoalTarget>()
                .ConstructUsing(dto => new GoalTarget(
                    new MetricValue(dto.TargetValue.Value, dto.TargetValue.DataType, dto.TargetValue.Unit),
                    dto.TargetDate,
                    dto.ThresholdType,
                    dto.SecondaryValue != null ? new MetricValue(dto.SecondaryValue.Value, dto.SecondaryValue.DataType, dto.SecondaryValue.Unit) : null))
                .ForAllMembers(opts => opts.Ignore());

            CreateMap<AssumptionValueDto, AssumptionValue>()
                .ConstructUsing(dto => new AssumptionValue(dto.Name, dto.Value, dto.Unit, dto.Description, dto.MinValue, dto.MaxValue))
                .ForAllMembers(opts => opts.Ignore());

            CreateMap<ProjectedAmountDto, ProjectedAmount>()
                .ConstructUsing(dto => new ProjectedAmount(dto.Amount, dto.PeriodIndex, dto.ProjectionDate, dto.ConfidenceLevel, dto.LowerBound, dto.UpperBound))
                .ForAllMembers(opts => opts.Ignore());

            CreateMap<BurnRateValueDto, BurnRateValue>()
                .ConstructUsing(dto => new BurnRateValue(dto.MonthlyAmount, dto.CalculationDate, dto.PeriodMonths))
                .ForAllMembers(opts => opts.Ignore());

            CreateMap<RunwayLengthDto, RunwayLength>()
                .ConstructUsing(dto => new RunwayLength(dto.Months, dto.DepletionDate, dto.CurrentBurnRate, dto.CurrentCashBalance))
                .ForAllMembers(opts => opts.Ignore());

            CreateMap<BreakEvenResultDto, BreakEvenResult>()
                .ConstructUsing(dto => new BreakEvenResult(dto.BreakEvenDate, dto.BreakEvenMonthIndex, dto.BreakEvenRevenue, 
                    dto.BreakEvenUnits, dto.FixedCosts, dto.VariableCostPerUnit, dto.PricePerUnit))
                .ForAllMembers(opts => opts.Ignore());

            // TrackedMetric mappings
            CreateMap<TrackedMetric, TrackedMetricDto>()
                .ForMember(dest => dest.DataPointCount, opt => opt.Ignore())
                .ForMember(dest => dest.ActiveGoalCount, opt => opt.Ignore())
                .ForMember(dest => dest.TrendPercentage, opt => opt.Ignore());

            CreateMap<TrackedMetric, TrackedMetricDetailDto>()
                .IncludeBase<TrackedMetric, TrackedMetricDto>()
                .ForMember(dest => dest.DefinitionName, opt => opt.Ignore())
                .ForMember(dest => dest.CategoryName, opt => opt.Ignore());

            CreateMap<MetricDataPoint, MetricDataPointDto>();
            CreateMap<MetricGoal, MetricGoalDto>()
                .ForMember(dest => dest.DaysRemaining, opt => opt.Ignore())
                .ForMember(dest => dest.ProgressPercentage, opt => opt.Ignore())
                .ForMember(dest => dest.IsOnTrack, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore());

            // MetricDefinition mappings
            CreateMap<MetricDefinition, MetricDefinitionDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.Ignore())
                .ForMember(dest => dest.UsageCount, opt => opt.Ignore());

            // MetricCategory mappings
            CreateMap<MetricCategory, MetricCategoryDto>()
                .ForMember(dest => dest.MetricCount, opt => opt.Ignore())
                .ForMember(dest => dest.DefinitionCount, opt => opt.Ignore());

            // FinancialModel mappings
            CreateMap<FinancialModel, FinancialModelDto>()
                .ForMember(dest => dest.ScenarioCount, opt => opt.Ignore())
                .ForMember(dest => dest.AssumptionSetCount, opt => opt.Ignore())
                .ForMember(dest => dest.ProjectionCount, opt => opt.Ignore())
                .ForMember(dest => dest.LastCalculatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CanCalculate, opt => opt.Ignore());

            CreateMap<FinancialModel, FinancialModelDetailDto>()
                .IncludeBase<FinancialModel, FinancialModelDto>();

            CreateMap<ModelScenario, ModelScenarioDto>()
                .ForMember(dest => dest.AssumptionSetNames, opt => opt.Ignore())
                .ForMember(dest => dest.ProjectionCount, opt => opt.Ignore())
                .ForMember(dest => dest.LastCalculatedAt, opt => opt.Ignore());

            CreateMap<AssumptionSet, AssumptionSetDto>()
                .ForMember(dest => dest.AssumptionCount, opt => opt.Ignore())
                .ForMember(dest => dest.IsValidated, opt => opt.Ignore());

            CreateMap<ProjectionResult, ProjectionResultDto>()
                .ForMember(dest => dest.ModelName, opt => opt.Ignore())
                .ForMember(dest => dest.ScenarioName, opt => opt.Ignore())
                .ForMember(dest => dest.TotalProjected, opt => opt.Ignore())
                .ForMember(dest => dest.AverageProjected, opt => opt.Ignore())
                .ForMember(dest => dest.RunwayMonths, opt => opt.Ignore())
                .ForMember(dest => dest.BreakEvenDate, opt => opt.Ignore());
        }
    }
}
