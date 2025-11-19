using System;
using System.Collections.Generic;
using NouAssist.TimeManagement.Calendar.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.TimeManagement.Calendar.Aggregates
{
    /// <summary>
    /// Template for creating recurring timeblocks
    /// </summary>
    public class TimeblockTemplate : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid OwnerId { get; private set; }
        public Guid? TenantId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; set; }
        public TimeblockType BlockType { get; set; }
        public int DurationMinutes { get; private set; }
        public string DefaultStartTime { get; set; } // HH:mm format
        public string Color { get; set; }
        public bool IsActive { get; private set; }

        private readonly List<int> _daysOfWeek = new();
        public IReadOnlyCollection<int> DaysOfWeek => _daysOfWeek.AsReadOnly();

        protected TimeblockTemplate() { }

        public TimeblockTemplate(
            Guid id,
            Guid ownerId,
            string title,
            int durationMinutes,
            Guid? tenantId = null,
            string description = null,
            TimeblockType blockType = TimeblockType.FocusWork,
            string defaultStartTime = null)
            : base(id)
        {
            OwnerId = ownerId;
            SetTitle(title);
            SetDuration(durationMinutes);
            TenantId = tenantId;
            Description = description;
            BlockType = blockType;
            DefaultStartTime = defaultStartTime;
            IsActive = true;
        }

        public void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(title, nameof(title), maxLength: 200);
        }

        public void SetDuration(int durationMinutes)
        {
            if (durationMinutes < 15 || durationMinutes > 480)
            {
                throw new BusinessException("Calendar:InvalidTimeblockDuration")
                    .WithData("min", 15)
                    .WithData("max", 480);
            }

            DurationMinutes = durationMinutes;
        }

        public void AddDayOfWeek(int dayOfWeek)
        {
            if (dayOfWeek < 0 || dayOfWeek > 6)
            {
                throw new ArgumentException("Day of week must be between 0 (Monday) and 6 (Sunday)", nameof(dayOfWeek));
            }

            if (!_daysOfWeek.Contains(dayOfWeek))
            {
                _daysOfWeek.Add(dayOfWeek);
            }
        }

        public void RemoveDayOfWeek(int dayOfWeek)
        {
            _daysOfWeek.Remove(dayOfWeek);
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public bool HasSchedule => _daysOfWeek.Count > 0;
        public bool IsDaily => _daysOfWeek.Count == 7;
        public double DurationHours => DurationMinutes / 60.0;
    }
}
