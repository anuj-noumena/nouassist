using AutoMapper;
using NouAssist.CoreProductivity.GoalTracking.Aggregates;
using NouAssist.CoreProductivity.GoalTracking.Dtos;
using NouAssist.CoreProductivity.GoalTracking.Entities;

namespace NouAssist.CoreProductivity.GoalTracking
{
    public class GoalsApplicationAutoMapperProfile : Profile
    {
        public GoalsApplicationAutoMapperProfile()
        {
            CreateMap<Goal, GoalDto>()
                .ForMember(dest => dest.ImportanceScore, opt => opt.MapFrom(src => src.Importance.Value))
                .ForMember(dest => dest.UrgencyScore, opt => opt.MapFrom(src => src.Urgency.Value))
                .ForMember(dest => dest.EisenhowerQuadrant, opt => opt.MapFrom(src => src.GetEisenhowerQuadrant()))
                .ForMember(dest => dest.IsOverdue, opt => opt.MapFrom(src => src.IsOverdue))
                .ForMember(dest => dest.DaysRemaining, opt => opt.MapFrom(src => src.GetDaysRemaining()))
                .ForMember(dest => dest.ProgressLevel, opt => opt.MapFrom(src => src.GetProgressLevel().ToString()))
                .ForMember(dest => dest.CompletedMilestonesCount, opt => opt.MapFrom(src => src.GetCompletedMilestonesCount()))
                .ForMember(dest => dest.MilestoneCompletionRate, opt => opt.MapFrom(src => src.GetMilestoneCompletionRate()));

            CreateMap<GoalMilestone, GoalMilestoneDto>()
                .ForMember(dest => dest.IsOverdue, opt => opt.MapFrom(src => src.IsOverdue));

            CreateMap<Achievement, AchievementDto>();
            
            CreateMap<UserAchievement, UserAchievementDto>()
                .ForMember(dest => dest.IsFullyUnlocked, opt => opt.MapFrom(src => src.IsFullyUnlocked))
                .ForMember(dest => dest.DaysSinceUnlock, opt => opt.MapFrom(src => src.DaysSinceUnlock));
        }
    }
}
