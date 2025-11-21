using System.Linq;
using NouAssist.Infrastructure.Reminders.Aggregates;
using NouAssist.Infrastructure.Reminders.ValueObjects;
using NouAssist.Notification.Reminders.Reminders.Dtos;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace NouAssist.Infrastructure.Reminders
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ReminderToReminderDtoMapper : MapperBase<Reminder, ReminderDto>
    {
        [MapperIgnoreTarget(nameof(ReminderDto.Status))]
        [MapperIgnoreTarget(nameof(ReminderDto.Priority))]
        [MapperIgnoreTarget(nameof(ReminderDto.RefEntityType))]
        [MapperIgnoreTarget(nameof(ReminderDto.HoursUntilReminder))]
        public override partial ReminderDto Map(Reminder source);

        [MapperIgnoreTarget(nameof(ReminderDto.Status))]
        [MapperIgnoreTarget(nameof(ReminderDto.Priority))]
        [MapperIgnoreTarget(nameof(ReminderDto.RefEntityType))]
        [MapperIgnoreTarget(nameof(ReminderDto.HoursUntilReminder))]
        public override partial void Map(Reminder source, ReminderDto destination);

        public override void AfterMap(Reminder source, ReminderDto destination)
        {
            destination.IsPending = source.IsPending;
            destination.IsOverdue = source.IsOverdue;
            destination.IsDue = source.IsDue;
            destination.IsRecurring = source.IsRecurring;
            destination.HoursUntilReminder = source.GetHoursUntilReminder();
            destination.Status = source.Status.ToString();
            destination.Priority = source.Priority.ToString();
            destination.RefEntityType = source.RefEntityType != null ? source.RefEntityType.ToString() : null;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ReminderScheduleToReminderScheduleDtoMapper : MapperBase<ReminderSchedule, ReminderScheduleDto>
    {
        [MapperIgnoreTarget(nameof(ReminderScheduleDto.RecurrencePattern))]
        [MapperIgnoreTarget(nameof(ReminderScheduleDto.RecurrenceDaysOfWeek))]
        public override partial ReminderScheduleDto Map(ReminderSchedule source);

        public override partial void Map(ReminderSchedule source, ReminderScheduleDto destination);

        public override void AfterMap(ReminderSchedule source, ReminderScheduleDto destination)
        {
            destination.RecurrencePattern = source.RecurrencePattern.ToString();
            destination.RecurrenceDaysOfWeek = source.RecurrenceDaysOfWeek.Select(d => d.ToString()).ToArray();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class SnoozeInfoToSnoozeInfoDtoMapper : MapperBase<SnoozeInfo, SnoozeInfoDto>
    {
        public override partial SnoozeInfoDto Map(SnoozeInfo source);

        public override partial void Map(SnoozeInfo source, SnoozeInfoDto destination);
    }
}
