using AutoMapper;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Aggregates;
using NouAssist.IntelligenceStrategy.StrategicPlanning.CrisisManagement.Dtos;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Entities;
using NouAssist.IntelligenceStrategy.StrategicPlanning.StrategicGoals.Dtos;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Mapping
{
    /// <summary>
    /// AutoMapper profile for Strategic Planning module
    /// </summary>
    public class StrategicPlanningApplicationAutoMapperProfile : Profile
    {
        public StrategicPlanningApplicationAutoMapperProfile()
        {
            // Strategic Goal mappings
            CreateMap<StrategicGoalDefinition, StrategicGoalDto>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Description.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description.Description))
                .ForMember(dest => dest.OverallProgress, opt => opt.MapFrom(src => src.OverallProgress))
                .ForMember(dest => dest.IsOnTrack, opt => opt.MapFrom(src => src.IsOnTrack));

            CreateMap<GoalTarget, GoalTargetDto>()
                .ForMember(dest => dest.MetricName, opt => opt.MapFrom(src => src.Metric.Name))
                .ForMember(dest => dest.TargetValue, opt => opt.MapFrom(src => src.Metric.Value))
                .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.Metric.Unit))
                .ForMember(dest => dest.TargetDate, opt => opt.MapFrom(src => src.Metric.TargetDate));

            // Crisis Management mappings
            CreateMap<CrisisManagementPlan, CrisisManagementPlanDto>()
                .ForMember(dest => dest.IsDueForReview, opt => opt.MapFrom(src => src.IsDueForReview));

            CreateMap<ContingencyPlanItem, ContingencyPlanItemDto>()
                .ForMember(dest => dest.RiskDescription, opt => opt.MapFrom(src => src.RiskScenario.Description))
                .ForMember(dest => dest.RiskLevel, opt => opt.MapFrom(src => src.RiskScenario.Level))
                .ForMember(dest => dest.ProbabilityScore, opt => opt.MapFrom(src => src.RiskScenario.ProbabilityScore))
                .ForMember(dest => dest.ImpactScore, opt => opt.MapFrom(src => src.RiskScenario.ImpactScore))
                .ForMember(dest => dest.RiskScore, opt => opt.MapFrom(src => src.RiskScenario.RiskScore));

            CreateMap<CrisisProtocolStep, CrisisProtocolStepDto>()
                .ForMember(dest => dest.ProcedureName, opt => opt.MapFrom(src => src.Procedure.Name))
                .ForMember(dest => dest.ProcedureDescription, opt => opt.MapFrom(src => src.Procedure.Description))
                .ForMember(dest => dest.RequiredCrisisLevel, opt => opt.MapFrom(src => src.Procedure.RequiredLevel))
                .ForMember(dest => dest.EstimatedDuration, opt => opt.MapFrom(src => src.Procedure.EstimatedDuration))
                .ForMember(dest => dest.ProcedureSteps, opt => opt.MapFrom(src => src.Procedure.Steps));

            // Sustainable Growth mappings
            CreateMap<SustainableGrowthFramework, SustainableGrowthFrameworkDto>();

            CreateMap<EsgGoalRecord, EsgGoalRecordDto>()
                .ForMember(dest => dest.TargetMetricName, opt => opt.MapFrom(src => src.Target.Name))
                .ForMember(dest => dest.TargetValue, opt => opt.MapFrom(src => src.Target.Value))
                .ForMember(dest => dest.TargetUnit, opt => opt.MapFrom(src => src.Target.Unit))
                .ForMember(dest => dest.TargetDate, opt => opt.MapFrom(src => src.Target.TargetDate))
                .ForMember(dest => dest.ProgressPercentage, opt => opt.MapFrom(src => src.ProgressPercentage))
                .ForMember(dest => dest.IsOnTrack, opt => opt.MapFrom(src => src.IsOnTrack));

            // Additional mappings for other aggregates would go here...
        }
    }
}
using AutoMapper;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Aggregates;
using NouAssist.IntelligenceStrategy.StrategicPlanning.CrisisManagement.Dtos;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Entities;
using NouAssist.IntelligenceStrategy.StrategicPlanning.IntegratedPlanning.Dtos;
using NouAssist.IntelligenceStrategy.StrategicPlanning.OpportunityManagement.Dtos;
using NouAssist.IntelligenceStrategy.StrategicPlanning.StrategicGoals.Dtos;
using NouAssist.IntelligenceStrategy.StrategicPlanning.SustainableGrowth.Dtos;
using NouAssist.IntelligenceStrategy.StrategicPlanning.TransitionPlanning.Dtos;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Mapping
{
    /// <summary>
    /// Complete AutoMapper profile for Strategic Planning module
    /// </summary>
    public class StrategicPlanningApplicationAutoMapperProfile : Profile
    {
        public StrategicPlanningApplicationAutoMapperProfile()
        {
            // Strategic Goal mappings
            CreateMap<StrategicGoalDefinition, StrategicGoalDto>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Description.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description.Description))
                .ForMember(dest => dest.OverallProgress, opt => opt.MapFrom(src => src.OverallProgress))
                .ForMember(dest => dest.IsOnTrack, opt => opt.MapFrom(src => src.IsOnTrack));

            CreateMap<GoalTarget, GoalTargetDto>()
                .ForMember(dest => dest.MetricName, opt => opt.MapFrom(src => src.Metric.Name))
                .ForMember(dest => dest.TargetValue, opt => opt.MapFrom(src => src.Metric.Value))
                .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.Metric.Unit))
                .ForMember(dest => dest.TargetDate, opt => opt.MapFrom(src => src.Metric.TargetDate));

            // Crisis Management mappings
            CreateMap<CrisisManagementPlan, CrisisManagementPlanDto>()
                .ForMember(dest => dest.IsDueForReview, opt => opt.MapFrom(src => src.IsDueForReview));

            CreateMap<ContingencyPlanItem, ContingencyPlanItemDto>()
                .ForMember(dest => dest.RiskDescription, opt => opt.MapFrom(src => src.RiskScenario.Description))
                .ForMember(dest => dest.RiskLevel, opt => opt.MapFrom(src => src.RiskScenario.Level))
                .ForMember(dest => dest.ProbabilityScore, opt => opt.MapFrom(src => src.RiskScenario.ProbabilityScore))
                .ForMember(dest => dest.ImpactScore, opt => opt.MapFrom(src => src.RiskScenario.ImpactScore))
                .ForMember(dest => dest.RiskScore, opt => opt.MapFrom(src => src.RiskScenario.RiskScore));

            CreateMap<CrisisProtocolStep, CrisisProtocolStepDto>()
                .ForMember(dest => dest.ProcedureName, opt => opt.MapFrom(src => src.Procedure.Name))
                .ForMember(dest => dest.ProcedureDescription, opt => opt.MapFrom(src => src.Procedure.Description))
                .ForMember(dest => dest.RequiredCrisisLevel, opt => opt.MapFrom(src => src.Procedure.RequiredLevel))
                .ForMember(dest => dest.EstimatedDuration, opt => opt.MapFrom(src => src.Procedure.EstimatedDuration))
                .ForMember(dest => dest.ProcedureSteps, opt => opt.MapFrom(src => src.Procedure.Steps));

            // Sustainable Growth mappings
            CreateMap<SustainableGrowthFramework, SustainableGrowthFrameworkDto>();

            CreateMap<EsgGoalRecord, EsgGoalRecordDto>()
                .ForMember(dest => dest.TargetMetricName, opt => opt.MapFrom(src => src.Target.Name))
                .ForMember(dest => dest.TargetValue, opt => opt.MapFrom(src => src.Target.Value))
                .ForMember(dest => dest.TargetUnit, opt => opt.MapFrom(src => src.Target.Unit))
                .ForMember(dest => dest.TargetDate, opt => opt.MapFrom(src => src.Target.TargetDate))
                .ForMember(dest => dest.ProgressPercentage, opt => opt.MapFrom(src => src.ProgressPercentage))
                .ForMember(dest => dest.IsOnTrack, opt => opt.MapFrom(src => src.IsOnTrack));

            // Integrated Planning mappings
            CreateMap<IntegratedHorizonPlan, IntegratedHorizonPlanDto>();

            CreateMap<TimelineEventMarker, TimelineEventMarkerDto>()
                .ForMember(dest => dest.EventDate, opt => opt.MapFrom(src => src.EventDate.Date))
                .ForMember(dest => dest.EventHorizon, opt => opt.MapFrom(src => src.EventDate.Horizon))
                .ForMember(dest => dest.DateDescription, opt => opt.MapFrom(src => src.EventDate.Description))
                .ForMember(dest => dest.IsEstimatedDate, opt => opt.MapFrom(src => src.EventDate.IsEstimated));

            // Opportunity Management mappings
            CreateMap<OpportunityManagementRecord, OpportunityManagementRecordDto>();

            CreateMap<OpportunityFilterRule, OpportunityFilterRuleDto>()
                .ForMember(dest => dest.CriterionName, opt => opt.MapFrom(src => src.Criterion.Name))
                .ForMember(dest => dest.CriterionDescription, opt => opt.MapFrom(src => src.Criterion.Description))
                .ForMember(dest => dest.MinThreshold, opt => opt.MapFrom(src => src.Criterion.MinThreshold))
                .ForMember(dest => dest.MaxThreshold, opt => opt.MapFrom(src => src.Criterion.MaxThreshold))
                .ForMember(dest => dest.CriterionWeight, opt => opt.MapFrom(src => src.Criterion.Weight));

            // Transition Planning mappings
            CreateMap<TransitionPlan, TransitionPlanDto>()
                .ForMember(dest => dest.OverallProgress, opt => opt.MapFrom(src => src.CalculateOverallProgress()))
                .ForMember(dest => dest.RemainingCost, opt => opt.MapFrom(src => src.EstimateRemainingCost()));

            CreateMap<CapabilityRoadmapItem, CapabilityRoadmapItemDto>()
                .ForMember(dest => dest.SkillName, opt => opt.MapFrom(src => src.Skill.Name))
                .ForMember(dest => dest.SkillCategory, opt => opt.MapFrom(src => src.Skill.Category))
                .ForMember(dest => dest.SkillDescription, opt => opt.MapFrom(src => src.Skill.Description))
                .ForMember(dest => dest.IsCompleted, opt => opt.MapFrom(src => src.IsCompleted))
                .ForMember(dest => dest.ProgressPercentage, opt => opt.MapFrom(src => src.ProgressPercentage));
        }
    }
}
