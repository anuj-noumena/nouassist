using NouAssist.TimeManagement.Meetings.Aggregates;
using NouAssist.TimeManagement.Meetings.Meetings.Dtos;
using NouAssist.TimeManagement.Meetings.ValueObjects;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace NouAssist.TimeManagement.Meetings
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class MeetingToMeetingDtoMapper : MapperBase<Meeting, MeetingDto>
    {
        [MapperIgnoreTarget(nameof(MeetingDto.AttendeeCount))]
        [MapperIgnoreTarget(nameof(MeetingDto.ConfirmedCount))]
        [MapperIgnoreTarget(nameof(MeetingDto.DeclinedCount))]
        [MapperIgnoreTarget(nameof(MeetingDto.AcceptanceRate))]
        [MapperIgnoreTarget(nameof(MeetingDto.DurationMinutes))]
        [MapperIgnoreTarget(nameof(MeetingDto.OwnerName))]
        [MapperIgnoreTarget(nameof(MeetingDto.OrganizerName))]
        public override partial MeetingDto Map(Meeting source);

        [MapperIgnoreTarget(nameof(MeetingDto.AttendeeCount))]
        [MapperIgnoreTarget(nameof(MeetingDto.ConfirmedCount))]
        [MapperIgnoreTarget(nameof(MeetingDto.DeclinedCount))]
        [MapperIgnoreTarget(nameof(MeetingDto.AcceptanceRate))]
        [MapperIgnoreTarget(nameof(MeetingDto.DurationMinutes))]
        [MapperIgnoreTarget(nameof(MeetingDto.OwnerName))]
        [MapperIgnoreTarget(nameof(MeetingDto.OrganizerName))]
        public override partial void Map(Meeting source, MeetingDto destination);

        public override void AfterMap(Meeting source, MeetingDto destination)
        {
            destination.AttendeeCount = source.GetAttendeeCount();
            destination.ConfirmedCount = source.GetConfirmedCount();
            destination.DeclinedCount = source.GetDeclinedCount();
            destination.AcceptanceRate = source.GetAttendeeCount() > 0
                ? (double)source.GetConfirmedCount() / source.GetAttendeeCount() * 100
                : 0;
            destination.DurationMinutes = source.Duration.Minutes;
            destination.IsUpcoming = source.IsUpcoming;
            destination.IsPast = source.IsPast;
            destination.IsHappeningNow = source.IsHappeningNow;
            destination.HasRecording = source.HasRecording;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class MeetingAttendeeToMeetingAttendeeDtoMapper : MapperBase<MeetingAttendee, MeetingAttendeeDto>
    {
        public override partial MeetingAttendeeDto Map(MeetingAttendee source);

        public override partial void Map(MeetingAttendee source, MeetingAttendeeDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class MeetingAgendaItemToMeetingAgendaItemDtoMapper : MapperBase<MeetingAgendaItem, MeetingAgendaItemDto>
    {
        [MapperIgnoreTarget(nameof(MeetingAgendaItemDto.PresenterName))]
        public override partial MeetingAgendaItemDto Map(MeetingAgendaItem source);

        [MapperIgnoreTarget(nameof(MeetingAgendaItemDto.PresenterName))]
        public override partial void Map(MeetingAgendaItem source, MeetingAgendaItemDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class MeetingActionItemToMeetingActionItemDtoMapper : MapperBase<MeetingActionItem, MeetingActionItemDto>
    {
        [MapperIgnoreTarget(nameof(MeetingActionItemDto.AssignedToName))]
        public override partial MeetingActionItemDto Map(MeetingActionItem source);

        [MapperIgnoreTarget(nameof(MeetingActionItemDto.AssignedToName))]
        public override partial void Map(MeetingActionItem source, MeetingActionItemDto destination);

        public override void AfterMap(MeetingActionItem source, MeetingActionItemDto destination)
        {
            destination.IsOverdue = source.IsOverdue;
        }
    }
}
