using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.Infrastructure.Reminders.ValueObjects
{
    public class SnoozeInfo : ValueObject
    {
        public DateTime OriginalRemindAt { get; init; }
        public DateTime NewRemindAt { get; init; }
        public int SnoozeDurationMinutes { get; init; }
        public int SnoozeCount { get; init; }
        public DateTime SnoozedAt { get; init; }

        public SnoozeInfo(
            DateTime originalRemindAt,
            DateTime newRemindAt,
            int snoozeDurationMinutes,
            int snoozeCount,
            DateTime snoozedAt)
        {
            OriginalRemindAt = originalRemindAt;
            NewRemindAt = newRemindAt;
            SnoozeDurationMinutes = snoozeDurationMinutes;
            SnoozeCount = snoozeCount;
            SnoozedAt = snoozedAt;
        }

        public bool IsMultipleSnooze => SnoozeCount > 1;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return OriginalRemindAt;
            yield return NewRemindAt;
            yield return SnoozeDurationMinutes;
            yield return SnoozeCount;
            yield return SnoozedAt;
        }
    }
}
