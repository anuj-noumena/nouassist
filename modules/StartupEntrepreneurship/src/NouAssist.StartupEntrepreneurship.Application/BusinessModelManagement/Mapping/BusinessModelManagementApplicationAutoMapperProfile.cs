using AutoMapper;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Aggregates;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.BusinessModelCanvas.Dtos;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Entities;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.RevenueModel.Dtos;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.StrategicPivot.Dtos;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.ValueObjects;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Mapping
{
    public class BusinessModelManagementApplicationAutoMapperProfile : Profile
    {
        public BusinessModelManagementApplicationAutoMapperProfile()
        {
            CreateBusinessModelCanvasMappings();
            CreateRevenueModelMappings();
            CreateStrategicPivotMappings();
            CreateValueObjectMappings();
        }

        private void CreateBusinessModelCanvasMappings()
        {
            // Business Model Canvas Aggregate
            CreateMap<BusinessModelCanvasDto, Aggregates.BusinessModelCanvas>().ReverseMap();

            // Canvas Version Entity
            CreateMap<CanvasVersion, CanvasVersionDto>();

            // Canvas Component Entity
            CreateMap<CanvasComponent, CanvasComponentDto>();

            // Associated Hypothesis Entity
            CreateMap<AssociatedHypothesis, HypothesisDto>();
        }

        private void CreateRevenueModelMappings()
        {
            // Revenue Model Aggregate
            CreateMap<Aggregates.RevenueModel, RevenueModelDto>();

            // Pricing Model Entity
            CreateMap<PricingModel, PricingModelDto>();

            // Revenue Scenario Entity
            CreateMap<RevenueScenario, RevenueScenarioDto>();

            // Go-to-Market Strategy Entity
            CreateMap<GoToMarketStrategy, GoToMarketStrategyDto>();
        }

        private void CreateStrategicPivotMappings()
        {
            // Strategic Pivot Aggregate
            CreateMap<Aggregates.StrategicPivot, StrategicPivotDto>();

            // Impact Metric Definition Entity
            CreateMap<ImpactMetricDefinition, ImpactMetricDefinitionDto>();
        }

        private void CreateValueObjectMappings()
        {
            // Component Content Value Object
            CreateMap<ComponentContent, ComponentContentDto>().ReverseMap();
            CreateMap<ComponentContentDto, ComponentContent>()
                .ConstructUsing(dto => new ComponentContent(dto.Text, dto.BulletPoints, dto.AdditionalProperties))
                .ForAllMembers(opts => opts.Ignore());

            // Validation State Value Object
            CreateMap<ValidationState, ValidationStateDto>();

            // Hypothesis Details Value Object
            CreateMap<HypothesisDetails, HypothesisDetailsDto>();

            // Pricing Tier Value Object
            CreateMap<PricingTier, PricingTierDto>().ReverseMap();
            CreateMap<PricingTierDto, PricingTier>()
                .ConstructUsing(dto => new PricingTier(dto.Name, dto.Price, dto.Currency, dto.BillingPeriod, dto.Features, dto.MaxUsers))
                .ForAllMembers(opts => opts.Ignore());

            // Scenario Parameters Value Object
            CreateMap<ScenarioParameters, ScenarioParametersDto>().ReverseMap();
            CreateMap<ScenarioParametersDto, ScenarioParameters>()
                .ConstructUsing(dto => new ScenarioParameters(
                    dto.CustomerCount,
                    dto.AverageRevenuePerUser,
                    dto.ChurnRate,
                    dto.GrowthRate,
                    dto.TimeHorizonMonths,
                    dto.CostPerCustomer,
                    dto.AdditionalParameters))
                .ForAllMembers(opts => opts.Ignore());

            // Projected Revenue Data Value Object
            CreateMap<ProjectedRevenueData, ProjectedRevenueDataDto>();

            // GTM Plan Element Value Object
            CreateMap<GtmPlanElement, GtmPlanElementDto>().ReverseMap();
            CreateMap<GtmPlanElementDto, GtmPlanElement>()
                .ConstructUsing(dto => new GtmPlanElement(
                    dto.ChannelType,
                    dto.ChannelName,
                    dto.Description,
                    dto.TargetAudience,
                    dto.EstimatedCost,
                    dto.Currency,
                    dto.Priority,
                    dto.StartDate,
                    dto.EndDate,
                    dto.KeyMetrics))
                .ForAllMembers(opts => opts.Ignore());

            // Pivot Rationale Value Object
            CreateMap<PivotRationale, PivotRationaleDto>().ReverseMap();
            CreateMap<PivotRationaleDto, PivotRationale>()
                .ConstructUsing(dto => new PivotRationale(
                    dto.Reason,
                    dto.ExpectedOutcome,
                    dto.KeyInsights,
                    dto.DataPoints,
                    dto.RiskAssessment,
                    dto.SuccessMetrics))
                .ForAllMembers(opts => opts.Ignore());

            // Impact Value Value Object
            CreateMap<ImpactValue, ImpactValueDto>().ReverseMap();
            CreateMap<ImpactValueDto, ImpactValue>()
                .ConstructUsing(dto => new ImpactValue(dto.MetricName, dto.Value, dto.Unit, dto.MeasuredAt, dto.MeasurementMethod, dto.Notes))
                .ForAllMembers(opts => opts.Ignore());
        }
    }
}
