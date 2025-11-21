using System.Linq;
using NouAssist.LearningDevelopment.Learning.Aggregates;
using NouAssist.LearningDevelopment.Learning.BookReadingHistories.Dtos;
using NouAssist.LearningDevelopment.Learning.Entities;
using NouAssist.LearningDevelopment.Learning.LearningNotes.Dtos;
using NouAssist.LearningDevelopment.Learning.LearningSessions.Dtos;
using NouAssist.LearningDevelopment.Learning.Mindmaps.Dtos;
using NouAssist.LearningDevelopment.Learning.Topics.Dtos;
using NouAssist.LearningDevelopment.Learning.ValueObjects;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace NouAssist.LearningDevelopment.Learning
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class TopicToTopicDtoMapper : MapperBase<Topic, TopicDto>
    {
        public override partial TopicDto Map(Topic source);

        public override partial void Map(Topic source, TopicDto destination);

        private readonly TopicToTopicDtoMapper _topicMapper = new();

        public override void AfterMap(Topic source, TopicDto destination)
        {
            destination.Tags = source.Tags.ToList();
            destination.Children = source.Children.Select(_topicMapper.Map).ToList();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class TopicToTopicSummaryDtoMapper : MapperBase<Topic, TopicSummaryDto>
    {
        public override partial TopicSummaryDto Map(Topic source);

        public override partial void Map(Topic source, TopicSummaryDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class TopicMetricsToTopicMetricsDtoMapper : MapperBase<TopicMetrics, TopicMetricsDto>
    {
        public override partial TopicMetricsDto Map(TopicMetrics source);

        public override partial void Map(TopicMetrics source, TopicMetricsDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class LearningSessionToLearningSessionDtoMapper : MapperBase<LearningSession, LearningSessionDto>
    {
        [MapperIgnoreTarget(nameof(LearningSessionDto.TotalTimeSpentMs))]
        public override partial LearningSessionDto Map(LearningSession source);

        public override partial void Map(LearningSession source, LearningSessionDto destination);

        private readonly PhaseMilestoneToPhaseMilestoneDtoMapper _milestoneMapper = new();

        public override void AfterMap(LearningSession source, LearningSessionDto destination)
        {
            destination.PhaseMilestones = source.PhaseMilestones.ToDictionary(kv => kv.Key, kv => _milestoneMapper.Map(kv.Value));
            destination.TimeTracking = source.TimeTracking.ToDictionary(kv => kv.Key, kv => kv.Value);
            destination.TotalTimeSpentMs = source.GetTotalTimeSpent();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class PhaseMilestoneToPhaseMilestoneDtoMapper : MapperBase<PhaseMilestone, PhaseMilestoneDto>
    {
        public override partial PhaseMilestoneDto Map(PhaseMilestone source);

        public override partial void Map(PhaseMilestone source, PhaseMilestoneDto destination);

        private readonly CycleProgressToCycleProgressDtoMapper _cycleMapper = new();

        public override void AfterMap(PhaseMilestone source, PhaseMilestoneDto destination)
        {
            destination.Questions = source.Questions;
            destination.SectionReflections = source.SectionReflections;
            destination.SpacedCycles = source.SpacedCycles.ToDictionary(kv => kv.Key, kv => _cycleMapper.Map(kv.Value));
            destination.MasteryValidation = source.MasteryValidation;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class BloomAssessmentResultToBloomAssessmentResultDtoMapper : MapperBase<BloomAssessmentResult, BloomAssessmentResultDto>
    {
        public override partial BloomAssessmentResultDto Map(BloomAssessmentResult source);

        public override partial void Map(BloomAssessmentResult source, BloomAssessmentResultDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class CycleProgressToCycleProgressDtoMapper : MapperBase<CycleProgress, CycleProgressDto>
    {
        public override partial CycleProgressDto Map(CycleProgress source);

        public override partial void Map(CycleProgress source, CycleProgressDto destination);

        public override void AfterMap(CycleProgress source, CycleProgressDto destination)
        {
            destination.Improvements = source.Improvements;
            destination.Reflection = source.Reflection;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class LearningNoteToLearningNoteDtoMapper : MapperBase<LearningNote, LearningNoteDto>
    {
        public override partial LearningNoteDto Map(LearningNote source);

        public override partial void Map(LearningNote source, LearningNoteDto destination);

        public override void AfterMap(LearningNote source, LearningNoteDto destination)
        {
            destination.KeyPoints = source.KeyPoints.ToList();
            destination.Metadata = source.Metadata.ToDictionary(kv => kv.Key, kv => kv.Value);
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class MindmapToMindmapDtoMapper : MapperBase<Mindmap, MindmapDto>
    {
        public override partial MindmapDto Map(Mindmap source);

        public override partial void Map(Mindmap source, MindmapDto destination);

        public override void AfterMap(Mindmap source, MindmapDto destination)
        {
            destination.Data = source.Data.ToDictionary(kv => kv.Key, kv => kv.Value);
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class BookReadingHistoryToBookReadingHistoryDtoMapper : MapperBase<BookReadingHistory, BookReadingHistoryDto>
    {
        public override partial BookReadingHistoryDto Map(BookReadingHistory source);

        public override partial void Map(BookReadingHistory source, BookReadingHistoryDto destination);
    }
}
