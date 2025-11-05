using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Calendar.Aggregates
{
    /// <summary>
    /// Manages user's weekly availability and specific date overrides
    /// </summary>
    public class UserAvailability : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid OwnerId { get; private set; }
        public Guid? TenantId { get; private set; }
        public string Timezone { get; set; }

        private readonly List<Entities.WeeklyAvailabilitySlot> _weeklySlots = new();
        public IReadOnlyCollection<Entities.WeeklyAvailabilitySlot> WeeklySlots => _weeklySlots.AsReadOnly();

        private readonly List<Entities.DateAvailabilityOverride> _dateOverrides = new();
        public IReadOnlyCollection<Entities.DateAvailabilityOverride> DateOverrides => _dateOverrides.AsReadOnly();

        protected UserAvailability() { }

        public UserAvailability(Guid id, Guid ownerId, Guid? tenantId = null, string timezone = "UTC")
            : base(id)
        {
            OwnerId = ownerId;
            TenantId = tenantId;
            Timezone = timezone ?? "UTC";
        }

        public void SetWeeklyAvailability(int dayOfWeek, string startTime, string endTime, bool isAvailable = true)
        {
            ValidateDayOfWeek(dayOfWeek);
            ValidateTimeFormat(startTime);
            ValidateTimeFormat(endTime);

            var existingSlot = _weeklySlots.FirstOrDefault(s => s.DayOfWeek == dayOfWeek);
            if (existingSlot != null)
            {
                _weeklySlots.Remove(existingSlot);
            }

            _weeklySlots.Add(new Entities.WeeklyAvailabilitySlot(
                Guid.NewGuid(),
                Id,
                dayOfWeek,
                startTime,
                endTime,
                isAvailable));
        }

        public void SetDateOverride(DateTime date, bool isAvailable, string reason = null, 
            string startTime = null, string endTime = null)
        {
            var normalizedDate = new DateTime(date.Year, date.Month, date.Day);
            
            var existingOverride = _dateOverrides.FirstOrDefault(o => o.Date.Date == normalizedDate);
            if (existingOverride != null)
            {
                _dateOverrides.Remove(existingOverride);
            }

            _dateOverrides.Add(new Entities.DateAvailabilityOverride(
                Guid.NewGuid(),
                Id,
                normalizedDate,
                isAvailable,
                reason,
                startTime,
                endTime));
        }

        public void RemoveDateOverride(DateTime date)
        {
            var normalizedDate = new DateTime(date.Year, date.Month, date.Day);
            _dateOverrides.RemoveAll(o => o.Date.Date == normalizedDate);
        }

        public bool IsAvailableOn(DateTime date)
        {
            var normalizedDate = new DateTime(date.Year, date.Month, date.Day);
            
            // Check date overrides first
            var dateOverride = _dateOverrides.FirstOrDefault(o => o.Date.Date == normalizedDate);
            if (dateOverride != null)
                return dateOverride.IsAvailable;

            // Check weekly availability
            var dayOfWeek = (int)date.DayOfWeek;
            var weeklySlot = _weeklySlots.FirstOrDefault(s => s.DayOfWeek == dayOfWeek);
            
            return weeklySlot?.IsAvailable ?? true;
        }

        private void ValidateDayOfWeek(int dayOfWeek)
        {
            if (dayOfWeek < 0 || dayOfWeek > 6)
                throw new ArgumentException("Day of week must be between 0 and 6", nameof(dayOfWeek));
        }

        private void ValidateTimeFormat(string time)
        {
            if (string.IsNullOrEmpty(time))
                return;

            if (!System.Text.RegularExpressions.Regex.IsMatch(time, @"^([01]\d|2[0-3]):([0-5]\d)$"))
                throw new BusinessException("Calendar:InvalidTimeFormat")
                    .WithData("format", "HH:mm");
        }
    }
}
