using NouAssist.WorkManagement.Reviews.Aggregates;
using NouAssist.WorkManagement.Reviews.Entities;
using NouAssist.WorkManagement.Reviews.Progress.Dtos;
using NouAssist.WorkManagement.Reviews.Reviews.Dtos;
using NouAssist.WorkManagement.Reviews.Services;
using NouAssist.WorkManagement.Reviews.Templates.Dtos;
using NouAssist.WorkManagement.Reviews.ValueObjects;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace NouAssist.WorkManagement.Reviews
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ReviewToReviewDtoMapper : MapperBase<Review, ReviewDto>
    {
        public override partial ReviewDto Map(Review source);

        public override partial void Map(Review source, ReviewDto destination);

        public override void AfterMap(Review source, ReviewDto destination)
        {
            destination.HasActionItems = source.HasActionItems;
            destination.HasScores = source.HasScores;
            destination.HasPeriod = source.HasPeriod;
            destination.HasReflections = source.HasReflections;
            destination.PeriodDurationDays = source.PeriodDurationDays;
            destination.TotalActionItems = source.TotalActionItems;
            destination.CompletedActionItems = source.CompletedActionItems;
            destination.OverdueActionItems = source.OverdueActionItems;
            destination.ActionItemsCompletionRate = source.ActionItemsCompletionRate;
            destination.TotalReviewItems = source.TotalReviewItems;
            destination.ReviewedItems = source.ReviewedItems;
            destination.RemainingItems = source.RemainingItems;
            destination.CalculatedCompletenessScore = source.CalculatedCompletenessScore;
            destination.Quality = source.Quality;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ReviewActionItemToReviewActionItemDtoMapper : MapperBase<ReviewActionItem, ReviewActionItemDto>
    {
        public override partial ReviewActionItemDto Map(ReviewActionItem source);

        public override partial void Map(ReviewActionItem source, ReviewActionItemDto destination);

        public override void AfterMap(ReviewActionItem source, ReviewActionItemDto destination)
        {
            destination.IsAssigned = source.IsAssigned;
            destination.HasDueDate = source.HasDueDate;
            destination.IsOverdue = source.IsOverdue;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ReviewItemToReviewItemDtoMapper : MapperBase<ReviewItem, ReviewItemDto>
    {
        public override partial ReviewItemDto Map(ReviewItem source);

        public override partial void Map(ReviewItem source, ReviewItemDto destination);

        public override void AfterMap(ReviewItem source, ReviewItemDto destination)
        {
            destination.HasDecision = source.HasDecision;
            destination.HasActionTaken = source.HasActionTaken;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ReviewTemplateToReviewTemplateDtoMapper : MapperBase<ReviewTemplate, ReviewTemplateDto>
    {
        public override partial ReviewTemplateDto Map(ReviewTemplate source);

        public override partial void Map(ReviewTemplate source, ReviewTemplateDto destination);

        public override void AfterMap(ReviewTemplate source, ReviewTemplateDto destination)
        {
            destination.IsCustomTemplate = source.IsCustomTemplate;
            destination.HasChecklist = source.HasChecklist;
            destination.HasReflectionPrompts = source.HasReflectionPrompts;
            destination.TotalChecklistItems = source.TotalChecklistItems;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ReviewChecklistStepToReviewChecklistStepDtoMapper : MapperBase<ReviewChecklistStep, ReviewChecklistStepDto>
    {
        public override partial ReviewChecklistStepDto Map(ReviewChecklistStep source);

        public override partial void Map(ReviewChecklistStep source, ReviewChecklistStepDto destination);

        public override void AfterMap(ReviewChecklistStep source, ReviewChecklistStepDto destination)
        {
            destination.ItemCount = source.ItemCount;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ReviewProgressToReviewProgressDtoMapper : MapperBase<ReviewProgress, ReviewProgressDto>
    {
        public override partial ReviewProgressDto Map(ReviewProgress source);

        public override partial void Map(ReviewProgress source, ReviewProgressDto destination);

        public override void AfterMap(ReviewProgress source, ReviewProgressDto destination)
        {
            destination.IsComplete = source.IsComplete;
            destination.IsInProgress = source.IsInProgress;
            destination.NotStarted = source.NotStarted;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ReviewQualityAnalysisToReviewQualityAnalysisDtoMapper : MapperBase<ReviewQualityAnalysis, ReviewQualityAnalysisDto>
    {
        public override partial ReviewQualityAnalysisDto Map(ReviewQualityAnalysis source);

        public override partial void Map(ReviewQualityAnalysis source, ReviewQualityAnalysisDto destination);
    }
}
