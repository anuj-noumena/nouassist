using AutoMapper;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Aggregates;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.ConversionFunnels.Dtos;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Entities;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.GrowthExperiments.Dtos;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.MarketingChannels.Dtos;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.ValueObjects;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Mapping
{
    public class GrowthExperimentationApplicationAutoMapperProfile : Profile
    {
        public GrowthExperimentationApplicationAutoMapperProfile()
        {
            // GrowthExperiment mappings
            CreateMap<GrowthExperiment, GrowthExperimentDto>();
            
            // Value object mappings for GrowthExperiment
            CreateMap<ExperimentGoal, ExperimentGoalDto>();
            CreateMap<CreateExperimentGoalDto, ExperimentGoal>()
                .ConstructUsing(dto => new ExperimentGoal(
                    dto.Description,
                    dto.ExpectedImprovement,
                    dto.MinimumViableImprovement
                ));
            
            CreateMap<TargetMetric, TargetMetricDto>();
            CreateMap<CreateTargetMetricDto, TargetMetric>()
                .ConstructUsing(dto => new TargetMetric(
                    dto.Name,
                    dto.Type,
                    dto.BaselineValue,
                    dto.TargetValue,
                    isPrimary: false // Set in service based on context
                ));
            
            CreateMap<StatisticalSignificance, StatisticalSignificanceDto>();
            
            CreateMap<ExperimentLearnings, ExperimentLearningsDto>();
            CreateMap<CreateExperimentLearningsDto, ExperimentLearnings>()
                .ForMember(dest => dest.DocumentedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DocumentedAt, opt => opt.Ignore());
            
            // Entity mappings for GrowthExperiment
            CreateMap<ExperimentHypothesis, ExperimentHypothesisDto>();
            CreateMap<CreateHypothesisDto, ExperimentHypothesis>()
                .ForMember(dest => dest.ExperimentId, opt => opt.Ignore());
            
            CreateMap<ExperimentVariant, ExperimentVariantDto>();
            CreateMap<CreateExperimentVariantDto, ExperimentVariant>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ExperimentId, opt => opt.Ignore());
            
            CreateMap<VariantSetup, VariantSetupDto>();
            CreateMap<CreateVariantSetupDto, VariantSetup>()
                .ConstructUsing(dto => new VariantSetup(
                    dto.TrafficAllocation,
                    dto.Configuration,
                    dto.Url
                ));
            
            CreateMap<ExperimentResultData, ExperimentResultDataDto>();
            
            CreateMap<ResultValue, ResultValueDto>();
            
            // MarketingChannelPerformance mappings
            CreateMap<MarketingChannelPerformance, MarketingChannelDto>();
            
            // Value object mappings for MarketingChannelPerformance
            CreateMap<ChannelDetails, ChannelDetailsDto>();
            CreateMap<CreateChannelDetailsDto, ChannelDetails>()
                .ConstructUsing(dto => new ChannelDetails(
                    dto.Platform,
                    dto.AccountId,
                    dto.Url,
                    dto.Settings
                ));
            
            CreateMap<CostPerAcquisition, CostPerAcquisitionDto>();
            
            CreateMap<ReturnOnAdSpend, ReturnOnAdSpendDto>();
            
            // Entity mappings for MarketingChannelPerformance
            CreateMap<ChannelMetric, ChannelMetricDto>();
            
            // ConversionFunnelDefinition mappings
            CreateMap<ConversionFunnelDefinition, ConversionFunnelDto>();
            
            // Value object mappings for ConversionFunnelDefinition
            CreateMap<StageDefinition, StageDefinitionDto>();
            
            CreateMap<ConversionRatePerStage, ConversionRatePerStageDto>();
            
            // Entity mappings for ConversionFunnelDefinition
            CreateMap<FunnelStage, FunnelStageDto>();
        }
    }
}
