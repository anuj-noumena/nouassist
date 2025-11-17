using AutoMapper;
using NouAssist.LearningDevelopment.Learning.Aggregates;
using NouAssist.LearningDevelopment.Learning.BookReadingHistories.Dtos;
using NouAssist.LearningDevelopment.Learning.Entities;
using NouAssist.LearningDevelopment.Learning.LearningNotes.Dtos;
using NouAssist.LearningDevelopment.Learning.LearningSessions.Dtos;
using NouAssist.LearningDevelopment.Learning.Mindmaps.Dtos;
using NouAssist.LearningDevelopment.Learning.Topics.Dtos;
using NouAssist.LearningDevelopment.Learning.ValueObjects;

namespace NouAssist.LearningDevelopment.Learning
{
    public class LearningApplicationAutoMapperProfile : Profile
    {
        public LearningApplicationAutoMapperProfile()
        {
            // Topic mappings
            CreateMap<Topic, TopicDto>()
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags))
                .ForMember(dest => dest.Children, opt => opt.MapFrom(src => src.Children));

            CreateMap<Topic, TopicSummaryDto>();
            CreateMap<TopicMetrics, TopicMetricsDto>();

            // LearningSession mappings
            CreateMap<LearningSession, LearningSessionDto>()
                .ForMember(dest => dest.PhaseMilestones, 
                    opt => opt.MapFrom(src => src.PhaseMilestones))
                .ForMember(dest => dest.TimeTracking, 
                    opt => opt.MapFrom(src => src.TimeTracking))
                .ForMember(dest => dest.TotalTimeSpentMs, 
                    opt => opt.MapFrom(src => src.GetTotalTimeSpent()));

            CreateMap<PhaseMilestone, PhaseMilestoneDto>()
                .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions))
                .ForMember(dest => dest.SectionReflections, opt => opt.MapFrom(src => src.SectionReflections))
                .ForMember(dest => dest.SpacedCycles, opt => opt.MapFrom(src => src.SpacedCycles))
                .ForMember(dest => dest.MasteryValidation, opt => opt.MapFrom(src => src.MasteryValidation));

            CreateMap<BloomAssessmentResult, BloomAssessmentResultDto>();
            CreateMap<CycleProgress, CycleProgressDto>()
                .ForMember(dest => dest.Improvements, opt => opt.MapFrom(src => src.Improvements))
                .ForMember(dest => dest.Reflection, opt => opt.MapFrom(src => src.Reflection));

            // LearningNote mappings
            CreateMap<LearningNote, LearningNoteDto>()
                .ForMember(dest => dest.KeyPoints, opt => opt.MapFrom(src => src.KeyPoints))
                .ForMember(dest => dest.Metadata, opt => opt.MapFrom(src => src.Metadata));

            // Mindmap mappings
            CreateMap<Mindmap, MindmapDto>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data));

            // BookReadingHistory mappings
            CreateMap<BookReadingHistory, BookReadingHistoryDto>();
        }
    }
}
