using System;
using NouAssist.WorkManagement.ProjectManagement.Enums;
using NouAssist.WorkManagement.ProjectManagement.Events;
using NouAssist.WorkManagement.ProjectManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;

namespace NouAssist.WorkManagement.ProjectManagement.Aggregates
{
    /// <summary>
    /// ProjectSprint - A time-boxed iteration within a project
    /// OPTIMIZED: Cleaner metrics, better state management
    /// </summary>
    public class ProjectSprint : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid ProjectId { get; private set; }
        public Guid? TenantId { get; private set; }
        public string Name { get; private set; }
        public string Goal { get; private set; }
        
        // === TIMELINE ===
        public SprintTimeline Timeline { get; private set; }
        
        // === METRICS (Value Object) ===
        public SprintMetrics Metrics { get; private set; }
        
        // === STATUS ===
        public SprintStatus Status { get; private set; }
        public string RetrospectiveNotes { get; set; }
        public string ReviewNotes { get; set; }
        
        protected ProjectSprint() { }
        
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
            Status = SprintStatus.Planned;
            
            Timeline = SprintTimeline.Create(startDate, endDate);
            Metrics = SprintMetrics.Initialize(velocityEstimate ?? 0);
        }
        
        public void UpdateDetails(string name, string goal)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            Goal = Check.NotNullOrWhiteSpace(goal, nameof(goal));
        }

        public void UpdateDates(DateTime startDate, DateTime endDate)
        {
            Timeline = Timeline.Update(startDate, endDate);
        }
        
        public void Start()
        {
            if (Status != SprintStatus.Planned)
            {
                throw new BusinessException("Sprints:AlreadyStarted");
            }
            
            Status = SprintStatus.InProgress;
            Timeline = Timeline.MarkStarted(DateTime.UtcNow);
        }
        
        public void Complete(string completedBy, string retrospectiveNotes = null)
        {
            if (Status == SprintStatus.Completed)
            {
                throw new BusinessException("Sprints:AlreadyCompleted");
            }
            
            Status = SprintStatus.Completed;
            Timeline = Timeline.MarkCompleted(DateTime.UtcNow);
            RetrospectiveNotes = retrospectiveNotes;
            Metrics = Metrics.Complete();
            
            AddDistributedEvent(new SprintCompletedEto(
                Id, ProjectId, TenantId, DateTime.UtcNow, 
                Metrics.PlannedStoryPoints, 
                Metrics.CompletedStoryPoints,
                Metrics.CompletionRate));
        }
        
        public void UpdateMetrics(int? planned = null, int? completed = null)
        {
            Metrics = Metrics.Update(planned, completed);
        }
        
        // Computed
        public bool IsActive => Timeline.IsActive && Status == SprintStatus.InProgress;
        public bool IsPast => Timeline.IsPast;
        public bool IsFuture => Timeline.IsFuture;
        public int? DaysRemaining => Timeline.DaysRemaining;
        public SprintHealth Health => CalculateHealth();
        
        private SprintHealth CalculateHealth()
        {
            if (Status == SprintStatus.Completed)
            {
                return SprintHealth.Completed;
            }

            if (!IsActive)
            {
                return SprintHealth.NotStarted;
            }

            var timeProgress = Timeline.TimeProgress;
            var workProgress = Metrics.CompletionRate / 100.0;
            
            if (workProgress >= timeProgress * 0.9)
            {
                return SprintHealth.OnTrack;
            }

            if (workProgress >= timeProgress * 0.7)
            {
                return SprintHealth.AtRisk;
            }

            return SprintHealth.BehindSchedule;
        }
    }
}