using System;
using NouAssist.Projects.Enums;
using NouAssist.Projects.Events;
using NouAssist.Projects.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Projects.Aggregates
{
    public class ProjectSprint : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid ProjectId { get; private set; }
        public Guid? TenantId { get; private set; }
        public string Name { get; private set; }
        public string Goal { get; private set; }
        public string ReleaseTheme { get; set; }

        // Dates
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        // Metrics
        private SprintMetrics _metrics;
        public int PlannedStoryPoints { get; private set; }
        public int CompletedStoryPoints { get; private set; }
        public int VelocityEstimate { get; private set; }
        public int ActualVelocity { get; private set; }

        // Status
        public SprintStatus Status { get; private set; }
        public DateTime? CompletedAt { get; private set; }
        public string CompletedBy { get; private set; }
        public string RetrospectiveNotes { get; set; }

        protected ProjectSprint() { /* ORM */ }

        public ProjectSprint(
            Guid id,
            Guid projectId,
            string name,
            string goal,
            DateTime startDate,
            DateTime endDate,
            Guid? tenantId,
            int? velocityEstimate = null) : base(id)
        {
            ProjectId = projectId;
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            Goal = Check.NotNullOrWhiteSpace(goal, nameof(goal));
            TenantId = tenantId;
            VelocityEstimate = velocityEstimate ?? 0;
            Status = SprintStatus.Planned;

            SetDates(startDate, endDate);
        }

        public void SetDates(DateTime startDate, DateTime endDate)
        {
            if (endDate <= startDate)
            {
                throw new BusinessException("Projects:InvalidSprintDates");
            }

            var duration = (endDate - startDate).Days;
            if (duration < 7 || duration > 30)
            {
                throw new BusinessException("Projects:InvalidSprintDuration", 
                    "Sprint duration must be between 7 and 30 days.");
            }

            StartDate = startDate;
            EndDate = endDate;
        }

        public void Start()
        {
            if (Status != SprintStatus.Planned)
            {
                throw new BusinessException("Projects:SprintAlreadyStarted");
            }

            Status = SprintStatus.InProgress;
        }

        public void Complete(string completedBy, string retrospectiveNotes = null)
        {
            if (Status == SprintStatus.Completed)
            {
                throw new BusinessException("Projects:SprintAlreadyCompleted");
            }

            Status = SprintStatus.Completed;
            CompletedAt = DateTime.UtcNow;
            CompletedBy = Check.NotNullOrWhiteSpace(completedBy, nameof(completedBy));
            RetrospectiveNotes = retrospectiveNotes;
            ActualVelocity = CompletedStoryPoints;

            _metrics = new SprintMetrics(
                PlannedStoryPoints,
                CompletedStoryPoints,
                VelocityEstimate,
                ActualVelocity);

            AddDistributedEvent(new SprintCompletedEto
            {
                SprintId = Id,
                ProjectId = ProjectId,
                TenantId = TenantId,
                CompletedAt = CompletedAt.Value,
                PlannedStoryPoints = PlannedStoryPoints,
                CompletedStoryPoints = CompletedStoryPoints,
                CompletionRate = CompletionRate
            });
        }

        public void UpdateStoryPoints(int? planned = null, int? completed = null)
        {
            if (planned.HasValue)
            {
                if (planned < 0)
                    throw new BusinessException("Projects:InvalidStoryPoints");
                PlannedStoryPoints = planned.Value;
            }

            if (completed.HasValue)
            {
                if (completed < 0 || completed > PlannedStoryPoints)
                    throw new BusinessException("Projects:InvalidCompletedStoryPoints");
                CompletedStoryPoints = completed.Value;
            }
        }

        // Computed Properties
        public int DurationInDays => (EndDate - StartDate).Days;

        public bool IsActive => Status == SprintStatus.InProgress &&
                                DateTime.UtcNow >= StartDate &&
                                DateTime.UtcNow <= EndDate;

        public bool IsPast => DateTime.UtcNow > EndDate;
        public bool IsFuture => DateTime.UtcNow < StartDate;

        public TimeSpan? TimeRemaining
        {
            get
            {
                if (Status == SprintStatus.Completed || IsPast) return null;
                return EndDate - DateTime.UtcNow;
            }
        }

        public int? DaysRemaining => TimeRemaining?.Days;

        public double CompletionRate => PlannedStoryPoints == 0 ? 0 : 
            (double)CompletedStoryPoints / PlannedStoryPoints * 100;

        public double VelocityAccuracy => VelocityEstimate == 0 ? 0 : 
            (double)ActualVelocity / VelocityEstimate * 100;

        public SprintHealth Health
        {
            get
            {
                if (Status == SprintStatus.Completed) return SprintHealth.Completed;
                if (!IsActive) return SprintHealth.NotStarted;

                var timeElapsedPercent = (DateTime.UtcNow - StartDate).TotalDays / DurationInDays;
                var progressPercent = CompletionRate / 100;

                if (progressPercent >= timeElapsedPercent * 0.9)
                    return SprintHealth.OnTrack;
                if (progressPercent >= timeElapsedPercent * 0.7)
                    return SprintHealth.AtRisk;
                return SprintHealth.BehindSchedule;
            }
        }
    }
}
