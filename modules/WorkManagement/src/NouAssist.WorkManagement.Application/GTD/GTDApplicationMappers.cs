using NouAssist.SharedKernel.ValueObjects;
using NouAssist.WorkManagement.GTD.Aggregates;
using NouAssist.WorkManagement.GTD.Dtos;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace NouAssist.WorkManagement.GTD
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class GtdHorizonToGtdHorizonDtoMapper : MapperBase<GtdHorizon, GtdHorizonDto>
    {
        public override partial GtdHorizonDto Map(GtdHorizon source);

        public override partial void Map(GtdHorizon source, GtdHorizonDto destination);

        public override void AfterMap(GtdHorizon source, GtdHorizonDto destination)
        {
            destination.NeedsReview = source.NeedsReview;
            destination.TotalRelatedEntities = source.TotalRelatedEntities;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class GtdHorizonAlignmentToGtdHorizonAlignmentDtoMapper : MapperBase<GtdHorizonAlignment, GtdHorizonAlignmentDto>
    {
        public override partial GtdHorizonAlignmentDto Map(GtdHorizonAlignment source);

        public override partial void Map(GtdHorizonAlignment source, GtdHorizonAlignmentDto destination);

        public override void AfterMap(GtdHorizonAlignment source, GtdHorizonAlignmentDto destination)
        {
            destination.HasStrongAlignment = source.HasStrongAlignment;
            destination.HasWeakAlignment = source.HasWeakAlignment;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class TicklerItemToTicklerItemDtoMapper : MapperBase<TicklerItem, TicklerItemDto>
    {
        [MapperIgnoreTarget(nameof(TicklerItemDto.ImportanceScore))]
        [MapperIgnoreTarget(nameof(TicklerItemDto.UrgencyScore))]
        public override partial TicklerItemDto Map(TicklerItem source);

        public override partial void Map(TicklerItem source, TicklerItemDto destination);

        public override void AfterMap(TicklerItem source, TicklerItemDto destination)
        {
            destination.ImportanceScore = source.Importance.Value;
            destination.UrgencyScore = source.Urgency.Value;
            destination.IsDue = source.IsDue;
            destination.RecurrencePattern = source.RecurrencePattern != null
                ? new RecurrenceRuleToRecurrenceRuleDtoMapper().Map(source.RecurrencePattern)
                : null;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class RecurrenceRuleToRecurrenceRuleDtoMapper : MapperBase<RecurrenceRule, RecurrenceRuleDto>
    {
        [MapperIgnoreTarget(nameof(RecurrenceRuleDto.Frequency))]
        public override partial RecurrenceRuleDto Map(RecurrenceRule source);

        public override partial void Map(RecurrenceRule source, RecurrenceRuleDto destination);

        public override void AfterMap(RecurrenceRule source, RecurrenceRuleDto destination)
        {
            destination.Frequency = source.Frequency.ToString();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class SomedayMaybeItemToSomedayMaybeItemDtoMapper : MapperBase<SomedayMaybeItem, SomedayMaybeItemDto>
    {
        [MapperIgnoreTarget(nameof(SomedayMaybeItemDto.ImportanceScore))]
        [MapperIgnoreTarget(nameof(SomedayMaybeItemDto.UrgencyScore))]
        public override partial SomedayMaybeItemDto Map(SomedayMaybeItem source);

        public override partial void Map(SomedayMaybeItem source, SomedayMaybeItemDto destination);

        public override void AfterMap(SomedayMaybeItem source, SomedayMaybeItemDto destination)
        {
            destination.ImportanceScore = source.Importance.Value;
            destination.UrgencyScore = source.Urgency.Value;
            destination.AgeInDays = source.AgeInDays;
            destination.NeedsReview = source.NeedsReview;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class WaitingForToWaitingForDtoMapper : MapperBase<WaitingFor, WaitingForDto>
    {
        public override partial WaitingForDto Map(WaitingFor source);

        public override partial void Map(WaitingFor source, WaitingForDto destination);

        public override void AfterMap(WaitingFor source, WaitingForDto destination)
        {
            destination.WaitingDays = source.WaitingDays;
            destination.IsFollowUpDue = source.IsFollowUpDue;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class FollowupToFollowupDtoMapper : MapperBase<Followup, FollowupDto>
    {
        public override partial FollowupDto Map(Followup source);

        public override partial void Map(Followup source, FollowupDto destination);

        public override void AfterMap(Followup source, FollowupDto destination)
        {
            destination.IsDue = source.IsDue;
        }
    }
}
