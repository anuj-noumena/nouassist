using AutoMapper;
using NouAssist.DecisionSupport.Aggregates;
using NouAssist.DecisionSupport.Dtos.Alternative;
using NouAssist.DecisionSupport.Dtos.Criterion;
using NouAssist.DecisionSupport.Dtos.DecisionModel;
using NouAssist.DecisionSupport.Dtos.ScenarioAnalysis;
using NouAssist.DecisionSupport.Dtos.ScenarioModel;
using NouAssist.DecisionSupport.Dtos.ScenarioOutcome;
using NouAssist.DecisionSupport.Dtos.Score;
using NouAssist.DecisionSupport.Dtos.StrategicInitiative;
using NouAssist.DecisionSupport.Entities;

namespace NouAssist.DecisionSupport.ObjectMapping
{
    public class DecisionSupportApplicationAutoMapperProfile : Profile
    {
        public DecisionSupportApplicationAutoMapperProfile()
        {
            // Decision Model mappings
            CreateMap<DecisionModelInstance, DecisionModelDto>();
            CreateMap<DecisionModelInstance, DecisionModelDetailDto>();
            
            // Criterion mappings
            CreateMap<Criterion, CriterionDto>()
                .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Weight.Value));
            
            // Alternative mappings
            CreateMap<Alternative, AlternativeDto>();
            
            // Score mappings
            CreateMap<AlternativeScore, AlternativeScoreDto>()
                .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Score.Value))
                .ForMember(dest => dest.CriterionName, opt => opt.Ignore())
                .ForMember(dest => dest.WeightedScore, opt => opt.Ignore());
            
            // Strategic Initiative mappings
            CreateMap<StrategicInitiativeProposal, StrategicInitiativeDto>();
            
            // Scenario Analysis mappings
            CreateMap<ScenarioAnalysis, ScenarioAnalysisDto>();
            
            // Scenario Model mappings
            CreateMap<Entities.ScenarioModel, ScenarioModelDto>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Description.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description.Description))
                .ForMember(dest => dest.Assumptions, opt => opt.MapFrom(src => src.Description.Assumptions))
                .ForMember(dest => dest.Likelihood, opt => opt.MapFrom(src => src.Likelihood.Likelihood))
                .ForMember(dest => dest.ConfidenceLevel, opt => opt.MapFrom(src => src.Likelihood.Confidence))
                .ForMember(dest => dest.LikelihoodRationale, opt => opt.MapFrom(src => src.Likelihood.Rationale))
                .ForMember(dest => dest.AverageImpact, opt => opt.Ignore());
            
            // Scenario Outcome mappings
            CreateMap<ScenarioOutcome, ScenarioOutcomeDto>()
                .ForMember(dest => dest.ImpactValue, opt => opt.MapFrom(src => src.Impact.Value))
                .ForMember(dest => dest.ImpactDescription, opt => opt.MapFrom(src => src.Impact.Description));
        }
    }
}
