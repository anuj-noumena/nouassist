using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.TimeManagement.Calendar.Aggregates
{
    public class TimeEntry : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid OwnerId { get; private set; }
        public Guid? TenantId { get; private set; }
        public Guid? TaskInstanceId { get; set; }
        public Guid? ProjectId { get; set; }
        public string Description { get; set; }
        public DateTime StartedAt { get; private set; }
        public DateTime? EndedAt { get; private set; }
        public int? DurationMinutes { get; private set; }
        public string Timezone { get; set; }
        public bool IsBillable { get; private set; }
        public string Notes { get; set; }

        private readonly List<string> _tags = new();
        public IReadOnlyCollection<string> Tags => _tags.AsReadOnly();

        protected TimeEntry() { }

        public TimeEntry(
            Guid id,
            Guid ownerId,
            DateTime startedAt,
            Guid? tenantId = null,
            DateTime? endedAt = null,
            Guid? taskInstanceId = null,
            Guid? projectId = null,
            string description = null,
            string timezone = "UTC")
            : base(id)
        {
            OwnerId = ownerId;
            TenantId = tenantId;
            StartedAt = startedAt;
            EndedAt = endedAt;
            TaskInstanceId = taskInstanceId;
            ProjectId = projectId;
            Description = description;
            Timezone = timezone ?? "UTC";
            IsBillable = false;
        }

        public void Stop()
        {
            if (EndedAt.HasValue)
                return;

            EndedAt = DateTime.UtcNow;
            DurationMinutes = (int)(EndedAt.Value - StartedAt).TotalMinutes;
        }

        public void UpdateDuration(int minutes)
        {
            DurationMinutes = minutes;
        }

        public void MakeBillable()
        {
            IsBillable = true;
        }

        public void MakeNonBillable()
        {
            IsBillable = false;
        }

        public void AddTag(string tag)
        {
            if (!_tags.Contains(tag))
                _tags.Add(tag);
        }

        public void RemoveTag(string tag)
        {
            _tags.Remove(tag);
        }

        public bool IsRunning => !EndedAt.HasValue;
        public TimeSpan CalculatedDuration => (EndedAt ?? DateTime.UtcNow) - StartedAt;
        public int CalculatedDurationMinutes => (int)CalculatedDuration.TotalMinutes;
        public double DurationHours => CalculatedDurationMinutes / 60.0;
    }
}
