using AutoMapper;
using NouAssist.Reviews.Aggregates;
using NouAssist.Reviews.Dtos;
using NouAssist.Reviews.Entities;
using NouAssist.Reviews.Progress.Dtos;
using NouAssist.Reviews.Services;
using NouAssist.Reviews.Templates.Dtos;
using NouAssist.Reviews.ValueObjects;

namespace NouAssist.Reviews
{
    public class ReviewsApplicationAutoMapperProfile : Profile
    {
        public ReviewsApplicationAutoMapperProfile()
        {
            // Review mappings
            CreateMap<Review, ReviewDto>()
                .ForMember(d => d.HasActionItems, opt => opt.MapFrom(s => s.HasActionItems))
                .ForMember(d => d.HasScores, opt => opt.MapFrom(s => s.HasScores))
                .ForMember(d => d.HasPeriod, opt => opt.MapFrom(s => s.HasPeriod))
                .ForMember(d => d.HasReflections, opt => opt.MapFrom(s => s.HasReflections))
                .ForMember(d => d.PeriodDurationDays, opt => opt.MapFrom(s => s.PeriodDurationDays))
                .ForMember(d => d.TotalActionItems, opt => opt.MapFrom(s => s.TotalActionItems))
                .ForMember(d => d.CompletedActionItems, opt => opt.MapFrom(s => s.CompletedActionItems))
                .ForMember(d => d.OverdueActionItems, opt => opt.MapFrom(s => s.OverdueActionItems))
                .ForMember(d => d.ActionItemsCompletionRate, opt => opt.MapFrom(s => s.ActionItemsCompletionRate))
                .ForMember(d => d.TotalReviewItems, opt => opt.MapFrom(s => s.TotalReviewItems))
                .ForMember(d => d.ReviewedItems, opt => opt.MapFrom(s => s.ReviewedItems))
                .ForMember(d => d.RemainingItems, opt => opt.MapFrom(s => s.RemainingItems))
                .ForMember(d => d.CalculatedCompletenessScore, opt => opt.MapFrom(s => s.CalculatedCompletenessScore))
                .ForMember(d => d.Quality, opt => opt.MapFrom(s => s.Quality));

            CreateMap<ReviewActionItem, ReviewActionItemDto>()
                .ForMember(d => d.IsAssigned, opt => opt.MapFrom(s => s.IsAssigned))
                .ForMember(d => d.HasDueDate, opt => opt.MapFrom(s => s.HasDueDate))
                .ForMember(d => d.IsOverdue, opt => opt.MapFrom(s => s.IsOverdue));

            CreateMap<ReviewItem, ReviewItemDto>()
                .ForMember(d => d.HasDecision, opt => opt.MapFrom(s => s.HasDecision))
                .ForMember(d => d.HasActionTaken, opt => opt.MapFrom(s => s.HasActionTaken));

            // Template mappings
            CreateMap<ReviewTemplate, ReviewTemplateDto>()
                .ForMember(d => d.IsCustomTemplate, opt => opt.MapFrom(s => s.IsCustomTemplate))
                .ForMember(d => d.HasChecklist, opt => opt.MapFrom(s => s.HasChecklist))
                .ForMember(d => d.HasReflectionPrompts, opt => opt.MapFrom(s => s.HasReflectionPrompts))
                .ForMember(d => d.TotalChecklistItems, opt => opt.MapFrom(s => s.TotalChecklistItems));

            CreateMap<ReviewChecklistStep, ReviewChecklistStepDto>()
                .ForMember(d => d.ItemCount, opt => opt.MapFrom(s => s.ItemCount));

            // Progress mappings
            CreateMap<ReviewProgress, ReviewProgressDto>()
                .ForMember(d => d.IsComplete, opt => opt.MapFrom(s => s.IsComplete))
                .ForMember(d => d.IsInProgress, opt => opt.MapFrom(s => s.IsInProgress))
                .ForMember(d => d.NotStarted, opt => opt.MapFrom(s => s.NotStarted));

            CreateMap<ReviewQualityAnalysis, ReviewQualityAnalysisDto>();
        }
    }
}
