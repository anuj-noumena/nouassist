using NouAssist.LearningDevelopment.SkillDevelopment.Aggregates;
using NouAssist.LearningDevelopment.SkillDevelopment.DevelopmentSessions.Dtos;
using NouAssist.LearningDevelopment.SkillDevelopment.Entities;
using NouAssist.LearningDevelopment.SkillDevelopment.Resources.Dtos;
using NouAssist.LearningDevelopment.SkillDevelopment.SkillLogs.Dtos;
using NouAssist.LearningDevelopment.SkillDevelopment.Skills.Dtos;
using NouAssist.LearningDevelopment.SkillDevelopment.SkillTrees.Dtos;
using NouAssist.LearningDevelopment.SkillDevelopment.ValueObjects;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace NouAssist.LearningDevelopment.SkillDevelopment
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class SkillToSkillDtoMapper : MapperBase<Skill, SkillDto>
    {
        [MapperIgnoreTarget(nameof(SkillDto.CurrentProficiencyScore))]
        [MapperIgnoreTarget(nameof(SkillDto.CurrentProficiencyAssessedAt))]
        public override partial SkillDto Map(Skill source);

        public override partial void Map(Skill source, SkillDto destination);

        public override void AfterMap(Skill source, SkillDto destination)
        {
            destination.CurrentProficiencyScore = source.CurrentProficiency.Score;
            destination.CurrentProficiencyAssessedAt = source.CurrentProficiency.AssessedAt;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class SkillToSkillSummaryDtoMapper : MapperBase<Skill, SkillSummaryDto>
    {
        [MapperIgnoreTarget(nameof(SkillSummaryDto.CurrentProficiencyScore))]
        [MapperIgnoreTarget(nameof(SkillSummaryDto.SubSkillCount))]
        public override partial SkillSummaryDto Map(Skill source);

        public override partial void Map(Skill source, SkillSummaryDto destination);

        public override void AfterMap(Skill source, SkillSummaryDto destination)
        {
            destination.CurrentProficiencyScore = source.CurrentProficiency.Score;
            destination.SubSkillCount = source.SubSkillIds.Count;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class SkillDevelopmentSessionToSkillDevelopmentSessionDtoMapper : MapperBase<SkillDevelopmentSession, SkillDevelopmentSessionDto>
    {
        [MapperIgnoreTarget(nameof(SkillDevelopmentSessionDto.SkillTitle))]
        [MapperIgnoreTarget(nameof(SkillDevelopmentSessionDto.TotalPracticeMinutes))]
        public override partial SkillDevelopmentSessionDto Map(SkillDevelopmentSession source);

        public override partial void Map(SkillDevelopmentSession source, SkillDevelopmentSessionDto destination);

        public override void AfterMap(SkillDevelopmentSession source, SkillDevelopmentSessionDto destination)
        {
            destination.TotalPracticeMinutes = source.GetTotalPracticeMinutes();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class SkillMilestoneToSkillMilestoneDtoMapper : MapperBase<SkillMilestone, SkillMilestoneDto>
    {
        [MapperIgnoreTarget(nameof(SkillMilestoneDto.TotalMinutes))]
        public override partial SkillMilestoneDto Map(SkillMilestone source);

        public override partial void Map(SkillMilestone source, SkillMilestoneDto destination);

        public override void AfterMap(SkillMilestone source, SkillMilestoneDto destination)
        {
            destination.TotalMinutes = source.TimeTracking.TotalMinutes;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class AssessmentResultToAssessmentResultDtoMapper : MapperBase<AssessmentResult, AssessmentResultDto>
    {
        [MapperIgnoreTarget(nameof(AssessmentResultDto.AverageScore))]
        public override partial AssessmentResultDto Map(AssessmentResult source);

        public override partial void Map(AssessmentResult source, AssessmentResultDto destination);

        public override void AfterMap(AssessmentResult source, AssessmentResultDto destination)
        {
            destination.AverageScore = source.GetAverageScore();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class PracticePlanItemToPracticePlanItemDtoMapper : MapperBase<PracticePlanItem, PracticePlanItemDto>
    {
        public override partial PracticePlanItemDto Map(PracticePlanItem source);

        public override partial void Map(PracticePlanItem source, PracticePlanItemDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class PracticeLogToPracticeLogDtoMapper : MapperBase<PracticeLog, PracticeLogDto>
    {
        public override partial PracticeLogDto Map(PracticeLog source);

        public override partial void Map(PracticeLog source, PracticeLogDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ApplicationExampleToApplicationExampleDtoMapper : MapperBase<ApplicationExample, ApplicationExampleDto>
    {
        public override partial ApplicationExampleDto Map(ApplicationExample source);

        public override partial void Map(ApplicationExample source, ApplicationExampleDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class SkillLogToSkillLogDtoMapper : MapperBase<SkillLog, SkillLogDto>
    {
        [MapperIgnoreTarget(nameof(SkillLogDto.SkillTitle))]
        public override partial SkillLogDto Map(SkillLog source);

        public override partial void Map(SkillLog source, SkillLogDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class SkillTreeToSkillTreeDtoMapper : MapperBase<SkillTree, SkillTreeDto>
    {
        [MapperIgnoreTarget(nameof(SkillTreeDto.SkillTitle))]
        public override partial SkillTreeDto Map(SkillTree source);

        public override partial void Map(SkillTree source, SkillTreeDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ResourceHistoryToResourceHistoryDtoMapper : MapperBase<ResourceHistory, ResourceHistoryDto>
    {
        [MapperIgnoreTarget(nameof(ResourceHistoryDto.RelatedSkillTitles))]
        public override partial ResourceHistoryDto Map(ResourceHistory source);

        public override partial void Map(ResourceHistory source, ResourceHistoryDto destination);
    }
}
