using AutoMapper;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Aggregates;
using NouAssist.RequirementsManagement.AnalysisPrioritization.AnalyzedRequirements.Dtos;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Entities;
using NouAssist.RequirementsManagement.AnalysisPrioritization.GlossaryTerms.Dtos;
using NouAssist.RequirementsManagement.AnalysisPrioritization.PrioritizationActivities.Dtos;
using NouAssist.RequirementsManagement.AnalysisPrioritization.ValueObjects;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.Mapping
{
    public class RequirementsAnalysisToolsApplicationAutoMapperProfile : Profile
    {
        public RequirementsAnalysisToolsApplicationAutoMapperProfile()
        {
            // AnalyzedRequirement mappings
            CreateMap<AnalyzedRequirement, AnalyzedRequirementDto>();
            CreateMap<RequirementAttributeInstance, RequirementAttributeDto>();
            CreateMap<RelationshipLink, RelationshipLinkDto>();

            // Value Object mappings
            CreateMap<AttributeValue, AttributeValueDto>().ReverseMap();
            CreateMap<ImpactAssessmentResult, ImpactAssessmentDto>();
            CreateMap<VoteValue, VoteValueDto>().ReverseMap();
            CreateMap<RankScore, RankScoreDto>();

            // GlossaryTerm mappings
            CreateMap<GlossaryTerm, GlossaryTermDto>()
                .ForMember(dest => dest.RelatedTermIds, opt => opt.MapFrom(src => src.GetRelatedTermIds()));

            // PrioritizationActivity mappings
            CreateMap<PrioritizationActivity, PrioritizationActivityDto>();
            CreateMap<StakeholderVote, StakeholderVoteDto>();

            // DTO to Value Object mappings with custom logic
            CreateMap<AttributeValueDto, AttributeValue>()
                .ConstructUsing(dto => CreateAttributeValue(dto))
                .ForAllMembers(opts => opts.Ignore());

            CreateMap<VoteValueDto, VoteValue>()
                .ConstructUsing(dto => CreateVoteValue(dto))
                .ForAllMembers(opts => opts.Ignore());
        }

        private static AttributeValue CreateAttributeValue(AttributeValueDto dto)
        {
            if (dto.NumericValue.HasValue)
            {
                return AttributeValue.FromNumber(dto.NumericValue.Value);
            }
            
            if (dto.BooleanValue.HasValue)
            {
                return AttributeValue.FromBoolean(dto.BooleanValue.Value);
            }
            
            if (dto.DateTimeValue.HasValue)
            {
                return AttributeValue.FromDateTime(dto.DateTimeValue.Value);
            }
            
            return AttributeValue.FromString(dto.StringValue);
        }

        private static VoteValue CreateVoteValue(VoteValueDto dto)
        {
            if (dto.Method == Enums.PrioritizationMethod.MoSCoW && !string.IsNullOrEmpty(dto.StringValue))
            {
                return VoteValue.CreateMoSCoWVote(dto.StringValue);
            }
            
            if (dto.Method == Enums.PrioritizationMethod.WSJF)
            {
                // For WSJF, we store the final score in NumericValue
                // In a real implementation, you might need separate fields for business value, time criticality, etc.
                return VoteValue.CreateNumericVote(dto.NumericValue, dto.Method);
            }
            
            return VoteValue.CreateNumericVote(dto.NumericValue, dto.Method);
        }
    }
}
