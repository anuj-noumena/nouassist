using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.CoreProductivity.ProjectManagement.Entities;
using NouAssist.CoreProductivity.ProjectManagement.Enums;
using NouAssist.CoreProductivity.ProjectManagement.Events;
using NouAssist.CoreProductivity.ProjectManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.CoreProductivity.ProjectManagement.Aggregates
{
    public class Project : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        // Identity & Relationships
        public Guid OwnerId { get; private set; }
        public Guid? ParentProjectId { get; private set; }
        public Guid? GoalId { get; private set; }
        public Guid? WorkflowSchemeId { get; set; }
        public Guid? ContextId { get; set; }
        public Guid? AreaId { get; set; }
        public Guid? TenantId { get; private set; }

        // Basic Info
        public string Title { get; private set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public string ExpectedOutcome { get; set; }

        // Status & Health
        public ProjectStatus Status { get; private set; }
        public bool IsArchived { get; private set; }
        public DateTime? ArchivedAt { get; private set; }

        // Priority & Classification
        public double Importance { get; private set; }
        public double Urgency { get; private set; }
        public EisenhowerQuadrant EisenhowerQuadrant { get; private set; }

        // Dates & Progress
        public DateTime? StartDate { get; private set; }
        public DateTime? DueDate { get; private set; }
        public DateTime? CompletedAt { get; private set; }
        public double Progress { get; private set; }
        public DateTime? LastReviewedAt { get; set; }

        // Success Criteria
        private readonly List<Guid> _successCriterionIds = new();
        public IReadOnlyCollection<Guid> SuccessCriterionIds => 
            _successCriterionIds.AsReadOnly();


        // Team Management
        public Guid? PrimaryTeamId { get; set; }
        private readonly List<Guid> _collaboratingTeamIds = new();
        public IReadOnlyCollection<Guid> CollaboratingTeamIds => 
            _collaboratingTeamIds.AsReadOnly();

        // Approval
        public bool RequiresApproval { get; set; }
        public Guid? ApprovalWorkflowId { get; set; }
        public Guid? CurrentApprovalRequestId { get; set; }
        
        private readonly List<ProjectSuccessCriterion> _successCriteria = new();

        public IReadOnlyCollection<ProjectSuccessCriterion> CriterionCompletions =>
            _successCriteria.AsReadOnly();

        protected Project() { /* ORM */ }

        public Project(
            Guid id,
            Guid ownerId,
            string title,
            Guid? tenantId,
            string description = null,
            string expectedOutcome = null,
            DateTime? startDate = null,
            DateTime? dueDate = null,
            double importance = 0.5,
            double urgency = 0.5) : base(id)
        {
            OwnerId = ownerId;
            Title = Check.NotNullOrWhiteSpace(title, nameof(title));
            TenantId = tenantId;
            Description = description;
            ExpectedOutcome = expectedOutcome;
            StartDate = startDate;
            DueDate = dueDate;
            Status = ProjectStatus.Planned;
            Progress = 0;

            SetPriority(importance, urgency);
            ValidateDateRange();

            AddDistributedEvent(new ProjectCreatedEto
            {
                ProjectId = Id,
                OwnerId = OwnerId,
                Title = Title,
                TenantId = TenantId,
                OccurredAt = DateTime.UtcNow
            });
        }

        #region Business Logic

        public void SetPriority(double importance, double urgency)
        {
            if (importance < 0 || importance > 1)
            {
                throw new BusinessException("Projects:InvalidImportance");
            }

            if (urgency < 0 || urgency > 1)
            {
                throw new BusinessException("Projects:InvalidUrgency");
            }

            Importance = importance;
            Urgency = urgency;
            EisenhowerQuadrant = CalculateQuadrant(importance, urgency);
        }

        public void UpdateDates(DateTime? startDate, DateTime? dueDate)
        {
            StartDate = startDate;
            DueDate = dueDate;
            ValidateDateRange();
        }

        public void Start()
        {
            if (Status != ProjectStatus.Planned && Status != ProjectStatus.OnHold)
            {
                throw new BusinessException("Projects:CannotStart", 
                    "Can only start planned or on-hold projects.");
            }

            var oldStatus = Status;
            Status = ProjectStatus.Active;
            StartDate ??= DateTime.UtcNow;

            AddStatusChangeEvent(oldStatus, Status);
        }

        public void Complete(string completedBy)
        {
            if (Status == ProjectStatus.Completed)
            {
                throw new BusinessException("Projects:AlreadyCompleted");
            }

            var oldStatus = Status;
            Status = ProjectStatus.Completed;
            CompletedAt = DateTime.UtcNow;
            Progress = 100;

            AddStatusChangeEvent(oldStatus, Status);
            AddDistributedEvent(new ProjectCompletedEto
            {
                ProjectId = Id,
                TenantId = TenantId,
                CompletedAt = CompletedAt.Value,
                FinalProgress = Progress,
                CompletedBy = completedBy
            });
        }

        public void Cancel()
        {
            if (Status == ProjectStatus.Completed || Status == ProjectStatus.Cancelled)
            {
                throw new BusinessException("Projects:CannotCancel");
            }

            var oldStatus = Status;
            Status = ProjectStatus.Cancelled;
            AddStatusChangeEvent(oldStatus, Status);
        }

        public void Pause()
        {
            if (Status != ProjectStatus.Active)
            {
                throw new BusinessException("Projects:CanOnlyPauseActive");
            }

            var oldStatus = Status;
            Status = ProjectStatus.OnHold;
            AddStatusChangeEvent(oldStatus, Status);
        }

        public void Archive()
        {
            if (Status == ProjectStatus.Active)
            {
                throw new BusinessException("Projects:CannotArchiveActive");
            }

            IsArchived = true;
            ArchivedAt = DateTime.UtcNow;
        }

        public void UpdateProgress(double newProgress)
        {
            if (newProgress < 0 || newProgress > 100)
            {
                throw new BusinessException("Projects:InvalidProgress", 
                    "Progress must be between 0 and 100.");
            }

            Progress = newProgress;
        }

        #endregion

        #region Success Criteria Management

        public void AddSuccessCriterion(Guid criterionId)
        {
            if (_successCriterionIds.Count >= 20) // DomainConstants.MaxSuccessCriteria
            {
                throw new BusinessException("Projects:MaxSuccessCriteria", 
                    "Maximum success criteria limit reached.");
            }

            if (!_successCriterionIds.Contains(criterionId))
            {
                _successCriterionIds.Add(criterionId);
            }
        }

        public void MarkSuccessCriterionMet(int index, string metBy)
        {
            if (index < 0 || index >= _successCriteria.Count)
            {
                throw new BusinessException("Projects:InvalidCriterionIndex");
            }

            _successCriteria[index].MarkAsMet(metBy);
        }

        public void RemoveSuccessCriterion(Guid criterionId)
        {
            _successCriterionIds.Remove(criterionId);
        }

        public bool HasSuccessCriterion(Guid criterionId)
        {
            return _successCriterionIds.Contains(criterionId);
        }

        #endregion

        #region Team Management

        public void AddCollaboratingTeam(Guid teamId)
        {
            if (!_collaboratingTeamIds.Contains(teamId))
            {
                _collaboratingTeamIds.Add(teamId);
            }
        }

        public void RemoveCollaboratingTeam(Guid teamId)
        {
            _collaboratingTeamIds.Remove(teamId);
        }

        #endregion

        #region Computed Properties

        public bool IsCompleted => Status == ProjectStatus.Completed;
        public bool IsCancelled => Status == ProjectStatus.Cancelled;
        public bool IsActive => Status == ProjectStatus.Active;
        public bool IsOnHold => Status == ProjectStatus.OnHold;
        public bool IsPlanned => Status == ProjectStatus.Planned;

        public bool HasStarted => StartDate.HasValue && DateTime.UtcNow >= StartDate.Value;

        public bool IsOverdue => DueDate.HasValue && 
                                 DateTime.UtcNow > DueDate.Value && 
                                 !IsCompleted;

        public TimeSpan? TimeRemaining
        {
            get
            {
                if (!DueDate.HasValue || IsCompleted)
                {
                    return null;
                }

                var remaining = DueDate.Value - DateTime.UtcNow;
                return remaining.TotalSeconds > 0 ? remaining : TimeSpan.Zero;
            }
        }

        public int? DaysRemaining => TimeRemaining?.Days;

        public bool HasSuccessCriteria => _successCriteria.Any();

        public int SuccessCriteriaMet => _successCriteria.Count(c => c.IsMet);

        public double SuccessCriteriaCompletionRate
        {
            get
            {
                if (_successCriteria.Count == 0)
                {
                    return 0;
                }

                return (double)SuccessCriteriaMet / _successCriteria.Count * 100;
            }
        }

        public bool AllSuccessCriteriaMet => 
            _successCriteria.Any() && _successCriteria.All(c => c.IsMet);

        public ProjectHealth Health => CalculateHealth();

        #endregion

        #region Private Methods

        private void ValidateDateRange()
        {
            if (StartDate.HasValue && DueDate.HasValue && DueDate < StartDate)
            {
                throw new BusinessException("Projects:InvalidDateRange", 
                    "Due date must be after start date.");
            }

            if (StartDate.HasValue && DueDate.HasValue)
            {
                var duration = (DueDate.Value - StartDate.Value).Days;
                if (duration > 3650) // ~10 years max
                {
                    throw new BusinessException("Projects:ExcessiveDuration", 
                        "Project duration exceeds maximum allowed.");
                }
            }
        }

        private static EisenhowerQuadrant CalculateQuadrant(double importance, double urgency)
        {
            if (importance >= 0.5 && urgency >= 0.5)
            {
                return EisenhowerQuadrant.Q1DoFirst;
            }

            if (importance >= 0.5 && urgency < 0.5)
            {
                return EisenhowerQuadrant.Q2Schedule;
            }

            if (importance < 0.5 && urgency >= 0.5)
            {
                return EisenhowerQuadrant.Q3Delegate;
            }

            return EisenhowerQuadrant.Q4Eliminate;
        }

        private ProjectHealth CalculateHealth()
        {
            if (IsCompleted)
            {
                return ProjectHealth.Completed;
            }

            if (IsCancelled)
            {
                return ProjectHealth.Cancelled;
            }

            if (IsOnHold)
            {
                return ProjectHealth.OnHold;
            }

            double healthScore = 0;
            int factors = 0;

            // Factor 1: Progress vs Time
            if (StartDate.HasValue && DueDate.HasValue && HasStarted)
            {
                var totalDuration = (DueDate.Value - StartDate.Value).TotalDays;
                var elapsed = (DateTime.UtcNow - StartDate.Value).TotalDays;
                var expectedProgress = elapsed / totalDuration * 100;
                var progressDelta = Progress - expectedProgress;

                if (progressDelta >= 0)
                {
                    healthScore += 1.0;
                }
                else if (progressDelta >= -10)
                {
                    healthScore += 0.5;
                }

                factors++;
            }

            // Factor 2: Success Criteria
            if (HasSuccessCriteria)
            {
                healthScore += SuccessCriteriaCompletionRate / 100;
                factors++;
            }

            // Factor 3: Not overdue
            if (!IsOverdue)
            {
                healthScore += 1.0;
                factors++;
            }

            var avgScore = factors > 0 ? healthScore / factors : 0.5;

            if (avgScore >= 0.7)
            {
                return ProjectHealth.Healthy;
            }

            if (avgScore >= 0.4)
            {
                return ProjectHealth.AtRisk;
            }

            return ProjectHealth.Critical;
        }

        private void AddStatusChangeEvent(ProjectStatus oldStatus, ProjectStatus newStatus)
        {
            AddDistributedEvent(new ProjectStatusChangedEto
            {
                ProjectId = Id,
                TenantId = TenantId,
                OldStatus = oldStatus.ToString(),
                NewStatus = newStatus.ToString(),
                ChangedAt = DateTime.UtcNow,
                ChangedBy = LastModifierId?.ToString()
            });
        }

        #endregion
    }
}
// UPDATE the Project class to manage entities instead of value objects

