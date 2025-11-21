using System.Linq;
using NouAssist.TimeManagement.Calendar.Aggregates;
using NouAssist.TimeManagement.Calendar.CalendarEvents.Dtos;
using NouAssist.TimeManagement.Calendar.Calendars.Dtos;
using NouAssist.TimeManagement.Calendar.Entities;
using NouAssist.TimeManagement.Calendar.SchedulingRequests.Dtos;
using NouAssist.TimeManagement.Calendar.Timeblocks.Dtos;
using NouAssist.TimeManagement.Calendar.TimeEntries.Dtos;
using NouAssist.TimeManagement.Calendar.ValueObjects;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace NouAssist.TimeManagement.Calendar
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class CalendarToCalendarDtoMapper : MapperBase<Aggregates.Calendar, CalendarDto>
    {
        public override partial CalendarDto Map(Aggregates.Calendar source);

        public override partial void Map(Aggregates.Calendar source, CalendarDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class CalendarEventToCalendarEventDtoMapper : MapperBase<CalendarEvent, CalendarEventDto>
    {
        public override partial CalendarEventDto Map(CalendarEvent source);

        public override partial void Map(CalendarEvent source, CalendarEventDto destination);

        private readonly LinkedEntityToLinkedEntityDtoMapper _linkedEntityMapper = new();

        public override void AfterMap(CalendarEvent source, CalendarEventDto destination)
        {
            destination.DurationInMinutes = source.DurationInMinutes;
            destination.Status = source.Status;
            destination.LinkedEntities = source.LinkedEntities.Select(_linkedEntityMapper.Map).ToList();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class RecurrenceRuleToRecurrenceRuleDtoMapper : TwoWayMapperBase<RecurrenceRule, RecurrenceRuleDto>
    {
        public override partial RecurrenceRuleDto Map(RecurrenceRule source);

        public override partial void Map(RecurrenceRule source, RecurrenceRuleDto destination);

        public override partial RecurrenceRule ReverseMap(RecurrenceRuleDto source);

        public override partial void ReverseMap(RecurrenceRuleDto source, RecurrenceRule destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class LinkedEntityToLinkedEntityDtoMapper : TwoWayMapperBase<LinkedEntity, LinkedEntityDto>
    {
        public override partial LinkedEntityDto Map(LinkedEntity source);

        public override partial void Map(LinkedEntity source, LinkedEntityDto destination);

        public override partial LinkedEntity ReverseMap(LinkedEntityDto source);

        public override partial void ReverseMap(LinkedEntityDto source, LinkedEntity destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class TimeblockToTimeblockDtoMapper : MapperBase<Timeblock, TimeblockDto>
    {
        public override partial TimeblockDto Map(Timeblock source);

        public override partial void Map(Timeblock source, TimeblockDto destination);

        private readonly TimeblockTaskToTimeblockTaskDtoMapper _taskMapper = new();

        public override void AfterMap(Timeblock source, TimeblockDto destination)
        {
            destination.DurationInMinutes = source.DurationInMinutes;
            destination.Tasks = source.Tasks.Select(_taskMapper.Map).ToList();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class TimeblockTaskToTimeblockTaskDtoMapper : MapperBase<TimeblockTask, TimeblockTaskDto>
    {
        public override partial TimeblockTaskDto Map(TimeblockTask source);

        public override partial void Map(TimeblockTask source, TimeblockTaskDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class SchedulingRequestToSchedulingRequestDtoMapper : MapperBase<SchedulingRequest, SchedulingRequestDto>
    {
        public override partial SchedulingRequestDto Map(SchedulingRequest source);

        public override partial void Map(SchedulingRequest source, SchedulingRequestDto destination);

        private readonly SchedulingSlotToSchedulingSlotDtoMapper _slotMapper = new();

        public override void AfterMap(SchedulingRequest source, SchedulingRequestDto destination)
        {
            destination.ParticipantIds = source.ParticipantIds.ToList();
            destination.Slots = source.Slots.Select(_slotMapper.Map).ToList();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class SchedulingSlotToSchedulingSlotDtoMapper : MapperBase<SchedulingSlot, SchedulingSlotDto>
    {
        public override partial SchedulingSlotDto Map(SchedulingSlot source);

        public override partial void Map(SchedulingSlot source, SchedulingSlotDto destination);

        public override void AfterMap(SchedulingSlot source, SchedulingSlotDto destination)
        {
            destination.DurationMinutes = source.DurationMinutes;
            destination.AvailableCount = source.AvailableCount;
            destination.AvailableParticipants = source.AvailableParticipants.ToList();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class TimeEntryToTimeEntryDtoMapper : MapperBase<TimeEntry, TimeEntryDto>
    {
        public override partial TimeEntryDto Map(TimeEntry source);

        public override partial void Map(TimeEntry source, TimeEntryDto destination);

        public override void AfterMap(TimeEntry source, TimeEntryDto destination)
        {
            destination.IsRunning = source.IsRunning;
            destination.CalculatedDurationMinutes = source.CalculatedDurationMinutes;
            destination.DurationHours = source.DurationHours;
            destination.Tags = source.Tags.ToList();
        }
    }
}
