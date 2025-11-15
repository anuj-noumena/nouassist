using AutoMapper;
using NouAssist.SkillDevelopment.Aggregates;
using NouAssist.SkillDevelopment.DevelopmentSessions.Dtos;
using NouAssist.SkillDevelopment.Entities;
using NouAssist.SkillDevelopment.Resources.Dtos;
using NouAssist.SkillDevelopment.SkillLogs.Dtos;
using NouAssist.SkillDevelopment.Skills.Dtos;
using NouAssist.SkillDevelopment.SkillTrees.Dtos;
using NouAssist.SkillDevelopment.ValueObjects;

namespace NouAssist.SkillDevelopment
{
    public class SkillDevelopmentApplicationAutoMapperProfile : Profile
    {
        public SkillDevelopmentApplicationAutoMapperProfile()
        {
            // Skill mappings
            CreateMap<Skill, SkillDto>()
                .ForMember(dest => dest.CurrentProficiencyScore, 
                    opt => opt.MapFrom(src => src.CurrentProficiency.Score))
                .ForMember(dest => dest.CurrentProficiencyAssessedAt, 
                    opt => opt.MapFrom(src => src.CurrentProficiency.AssessedAt));

            CreateMap<Skill, SkillSummaryDto>()
                .ForMember(dest => dest.CurrentProficiencyScore, 
                    opt => opt.MapFrom(src => src.CurrentProficiency.Score))
                .ForMember(dest => dest.SubSkillCount, 
                    opt => opt.MapFrom(src => src.SubSkillIds.Count));

            // Session mappings
            CreateMap<SkillDevelopmentSession, SkillDevelopmentSessionDto>()
                .ForMember(dest => dest.SkillTitle, opt => opt.Ignore())
                .ForMember(dest => dest.TotalPracticeMinutes, 
                    opt => opt.MapFrom(src => src.GetTotalPracticeMinutes()));

            CreateMap<SkillMilestone, SkillMilestoneDto>()
                .ForMember(dest => dest.TotalMinutes, 
                    opt => opt.MapFrom(src => src.TimeTracking.TotalMinutes));

            CreateMap<AssessmentResult, AssessmentResultDto>()
                .ForMember(dest => dest.AverageScore, 
                    opt => opt.MapFrom(src => src.GetAverageScore()));

            CreateMap<PracticePlanItem, PracticePlanItemDto>();
            CreateMap<PracticeLog, PracticeLogDto>();
            CreateMap<ApplicationExample, ApplicationExampleDto>();

            // SkillLog mappings
            CreateMap<SkillLog, SkillLogDto>()
                .ForMember(dest => dest.SkillTitle, opt => opt.Ignore());

            // SkillTree mappings
            CreateMap<SkillTree, SkillTreeDto>()
                .ForMember(dest => dest.SkillTitle, opt => opt.Ignore());

            // ResourceHistory mappings
            CreateMap<ResourceHistory, ResourceHistoryDto>()
                .ForMember(dest => dest.RelatedSkillTitles, opt => opt.Ignore());
        }
    }
}
