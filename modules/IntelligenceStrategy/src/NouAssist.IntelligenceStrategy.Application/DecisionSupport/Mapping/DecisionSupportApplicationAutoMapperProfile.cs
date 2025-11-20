using AutoMapper;
using NouAssist.IntelligenceStrategy.DecisionSupport.Aggregates;
using NouAssist.IntelligenceStrategy.DecisionSupport.DecisionModels.Dtos;
using NouAssist.IntelligenceStrategy.DecisionSupport.Entities;
using NouAssist.IntelligenceStrategy.DecisionSupport.StrategicInitiatives.Dtos;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.Mapping
{
    /// <summary>
    /// AutoMapper profile for the Decision Support application layer
    /// </summary>
    public class DecisionSupportApplicationAutoMapperProfile : Profile
    {
        /// <summary>
        /// Creates a new instance of <see cref="DecisionSupportApplicationAutoMapperProfile"/>
        /// </summary>
        public DecisionSupportApplicationAutoMapperProfile()
        {
            // StrategicInitiativeProposal mappings
            CreateMap<StrategicInitiativeProposal, StrategicInitiativeDto>()
                .ForMember(dest => dest.Rationale, opt => opt.MapFrom(src => src.Rationale.Value))
                .ForMember(dest => dest.ExpectedOutcomes, opt => opt.MapFrom(src => src.ExpectedOutcomes.Value))
                .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(src => src.ProjectId != null ? src.ProjectId.Value : (Guid?)null))
                .ForMember(dest => dest.SupportingAnalyses, opt => opt.MapFrom(src => src.SupportingAnalyses));
                
            CreateMap<StrategicInitiativeProposal, StrategicInitiativeListDto>()
                .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(src => src.ProjectId != null ? src.ProjectId.Value : (Guid?)null));
                
            // AnalysisReference mappings
            CreateMap<AnalysisReference, AnalysisReferenceDto>();
            
            // DecisionModelInstance mappings
            CreateMap<DecisionModelInstance, DecisionModelDto>();
            CreateMap<DecisionModelInstance, DecisionModelListDto>()
                .ForMember(dest => dest.CriteriaCount, opt => opt.MapFrom(src => src.Criteria.Count))
                .ForMember(dest => dest.AlternativesCount, opt => opt.MapFrom(src => src.Alternatives.Count));
                
            // Criterion mappings
            CreateMap<Criterion, CriterionDto>();
            
            // Alternative mappings
            CreateMap<Alternative, AlternativeDto>()
                .ForMember(dest => dest.IsSelected, opt => opt.Ignore())
                .ForMember(dest => dest.TotalScore, opt => opt.Ignore());
                
            // AlternativeScore mappings
            CreateMap<AlternativeScore, AlternativeScoreDto>()
                .ForMember(dest => dest.WeightedScore, opt => opt.Ignore());
        }
    }
}
