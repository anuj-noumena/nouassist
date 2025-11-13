using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.WorkManagement.ProjectManagement.Entities;
using NouAssist.WorkManagement.ProjectManagement.Enums;
using NouAssist.WorkManagement.ProjectManagement.Events;
using NouAssist.WorkManagement.ProjectManagement.ValueObjects;
using NouAssist.WorkManagement.TaskManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;

namespace NouAssist.WorkManagement.ProjectManagement.Aggregates
{
    /// <summary>
    /// Project Aggregate - A specific outcome with measurable success criteria
    /// OPTIMIZED: Cleaner boundaries, consistent patterns, GTD-aligned
    /// </summary>
    public class Project : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        // === IDENTITY & RELATIONSHIPS ===
        public Guid OwnerId { get; private set; }
        public Guid? TenantId { get; private set; }
        
        // === CONTENT ===
        public string Title { get; private set; }
        public string Description { get; set; }
        public string Vision { get; set; } // What success looks like
        public string Purpose { get; set; } // Why this project exists
        public string ExpectedOutcome { get; set; }
        
        // === ORGANIZATION ===
        // Project can belong to an Area (GTD: Projects support Areas)
        public Guid? AreaId { get; set; } // Which area of responsibility
        public Guid? GoalId { get; private set; } // Aligned to which strategic goal
        
        // === WORKFLOW ===
        public Guid? WorkflowSchemeId { get; set; }
        public ProjectStatus Status { get; private set; }
        public bool IsArchived { get; private set; }
        public DateTime? ArchivedAt { get; private set; }
        
        // === HIERARCHY (Simplified) ===
        public Guid? ParentProjectId { get; private set; }
        private int _level; // Denormalized for query performance
        private string _hierarchyPath; // e.g., "/parent/child"
        
        // === PRIORITY (Use same matrix as tasks) ===
        public ImportanceUrgencyMatrix Priority { get; private set; }
        
        // === TIMELINE ===
        public ProjectTimeline Timeline { get; private set; }
        
        // === PROGRESS ===
        public double Progress { get; private set; }
        public DateTime? LastReviewedAt { get; set; }
        
        // === TEAM (Keep simple - just IDs) ===
        public Guid? PrimaryTeamId { get; set; }
        private readonly List<Guid> _collaboratingTeamIds = new();
        public IReadOnlyCollection<Guid> CollaboratingTeamIds => _collaboratingTeamIds.AsReadOnly();
        
        // === APPROVAL (Simplified) ===
        public Guid? ApprovalRequestId { get; set; } // Link to approval if needed
        
        // === SUCCESS CRITERIA (Entity collection - own lifecycle) ===
        private readonly List<ProjectSuccessCriterion> _successCriteria = new();
        public IReadOnlyCollection<ProjectSuccessCriterion> SuccessCriteria => _successCriteria.AsReadOnly();
        
        // === COMPUTED PROPERTIES ===
        public bool IsCompleted => Status == ProjectStatus.Completed;
        public bool IsActive => Status == ProjectStatus.Active;
        public bool IsOnHold => Status == ProjectStatus.OnHold;
        public bool IsOverdue => Timeline.IsOverdue;
        public bool HasSuccessCriteria => _successCriteria.Any();
        public int SuccessCriteriaMet => _successCriteria.Count(c => c.IsMet);
        public double SuccessCriteriaCompletionRate => 
            _successCriteria.Count == 0 ? 0 : (double)SuccessCriteriaMet / _successCriteria.Count * 100;
        public bool AllSuccessCriteriaMet => 
            _successCriteria.Any() && _successCriteria.All(c => c.IsMet);
        public ProjectHealth Health => CalculateHealth();
        
        protected Project() { }
        
        public Project(
            Guid id,
            Guid ownerId,
            string title,
            Guid? tenantId,
            Guid? areaId = null,
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
            AreaId = areaId;
            Description = description;
            ExpectedOutcome = expectedOutcome;
            Status = ProjectStatus.Planned;
            Progress = 0;
            
            Priority = new ImportanceUrgencyMatrix(importance, urgency);
            Timeline = ProjectTimeline.Create(startDate, dueDate);
            
            _level = 0;
            _hierarchyPath = $"/{id}";
            
            AddDistributedEvent(new ProjectCreatedEto
            {
                OwnerId = ownerId,
                ProjectId = id,
                Title =  title,
                OccurredAt= DateTime.UtcNow,
                AreaId = areaId
            });
        }
        
        // === BUSINESS METHODS ===
        
        public void UpdatePriority(double importance, double urgency)
        {
            Priority = new ImportanceUrgencyMatrix(importance, urgency);
        }
        
        public void UpdateTimeline(DateTime? startDate, DateTime? dueDate)
        {
            Timeline = Timeline.Update(startDate, dueDate);
        }
        
        public void Start()
        {
            if (Status != ProjectStatus.Planned && Status != ProjectStatus.OnHold)
            {
                throw new BusinessException("Projects:CannotStart");
            }
            
            var oldStatus = Status;
            Status = ProjectStatus.Active;
            Timeline = Timeline.MarkStarted(DateTime.UtcNow);
            
            AddDistributedEvent(new ProjectStatusChangedEto
            {
                OldStatus = oldStatus,
                NewStatus = Status,
                ProjectId = Id,
                ChangedAt = DateTime.UtcNow
            });
        }
        
        public void Complete(string completedBy)
        {
            if (Status == ProjectStatus.Completed)
            {
                throw new BusinessException("Projects:AlreadyCompleted");
            }
            
            // Business rule: All required success criteria must be met
            var unmetRequired = _successCriteria
                .Where(sc => !sc.IsMet && sc.Weight >= 8) // Weight 8+ = required
                .ToList();
            
            if (unmetRequired.Any())
            {
                throw new BusinessException("Projects:RequiredCriteriaNotMet",
                    $"Cannot complete project. {unmetRequired.Count} required criteria not met.");
            }
            
            var oldStatus = Status;
            Status = ProjectStatus.Completed;
            Timeline = Timeline.MarkCompleted(DateTime.UtcNow);
            Progress = 100;
            
            AddDistributedEvent(new ProjectCompletedEto(
                Id, DateTime.UtcNow, Progress, SuccessCriteriaCompletionRate));
        }
        
        public void Pause()
        {
            if (Status != ProjectStatus.Active)
            {
                throw new BusinessException("Projects:CanOnlyPauseActive");
            }
            
            Status = ProjectStatus.OnHold;
        }
        
        public void Cancel()
        {
            if (Status == ProjectStatus.Completed || Status == ProjectStatus.Cancelled)
            {
                throw new BusinessException("Projects:CannotCancel");
            }
            
            Status = ProjectStatus.Cancelled;
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
                throw new BusinessException("Projects:InvalidProgress");
            }
            
            Progress = newProgress;
        }
        
        // === SUCCESS CRITERIA MANAGEMENT ===
        
        public void AddSuccessCriterion(
            Guid projectSuccessCriterionId,
            string title,
            SuccessCriterionType type,
            string description = null,
            string measurementMethod = null,
            string targetValue = null,
            double weight = 1.0)
        {
            if (_successCriteria.Count >= 20)
            {
                throw new BusinessException("Projects:MaxSuccessCriteria");
            }
            
            var criterion = new ProjectSuccessCriterion(
                projectSuccessCriterionId,
                Id,
                title,
                type,
                description,
                measurementMethod,
                targetValue,
                weight);
            
            _successCriteria.Add(criterion);
        }
        
        public void RemoveSuccessCriterion(Guid criterionId)
        {
            var criterion = _successCriteria.FirstOrDefault(c => c.Id == criterionId);
            if (criterion == null)
            {
                throw new BusinessException("Projects:CriterionNotFound");
            }
            
            _successCriteria.Remove(criterion);
        }
        
        public void MarkCriterionAsMet(Guid criterionId, string metBy, string notes = null)
        {
            var criterion = _successCriteria.FirstOrDefault(c => c.Id == criterionId);
            if (criterion == null)
            {
                throw new BusinessException("Projects:CriterionNotFound");
            }
            
            criterion.MarkAsMet(metBy, notes);
            
            // Recalculate progress based on weighted criteria
            RecalculateProgressFromCriteria();
        }
        
        private void RecalculateProgressFromCriteria()
        {
            if (_successCriteria.Count == 0)
            {
                return;
            }

            var totalWeight = _successCriteria.Sum(c => c.Weight);
            var metWeight = _successCriteria.Where(c => c.IsMet).Sum(c => c.Weight);
            
            Progress = totalWeight > 0 ? (metWeight / totalWeight * 100) : 0;
        }
        
        // === HIERARCHY ===
        
        public void SetParent(Guid parentProjectId, int parentLevel, string parentPath)
        {
            if (parentProjectId == Id)
            {
                throw new BusinessException("Projects:CannotBeOwnParent");
            }
            
            ParentProjectId = parentProjectId;
            _level = parentLevel + 1;
            _hierarchyPath = $"{parentPath}/{Id}";
        }
        
        public void ClearParent()
        {
            ParentProjectId = null;
            _level = 0;
            _hierarchyPath = $"/{Id}";
        }
        
        // === TEAM MANAGEMENT ===
        
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
        
        // === HEALTH CALCULATION ===
        
        private ProjectHealth CalculateHealth()
        {
            if (IsCompleted)
            {
                return ProjectHealth.Completed;
            }

            if (Status == ProjectStatus.Cancelled)
            {
                return ProjectHealth.Cancelled;
            }

            if (Status == ProjectStatus.OnHold)
            {
                return ProjectHealth.OnHold;
            }

            var healthScore = 0.0;
            var factors = 0;
            
            // Factor 1: Progress vs Timeline
            if (Timeline.HasStarted && Timeline.PlannedDueDate.HasValue)
            {
                var timeProgress = Timeline.TimeProgress;
                var workProgress = Progress / 100.0;
                
                if (workProgress >= timeProgress * 0.9)
                {
                    healthScore += 1.0;
                }
                else if (workProgress >= timeProgress * 0.7)
                {
                    healthScore += 0.5;
                }
                
                factors++;
            }
            
            // Factor 2: Success Criteria
            if (HasSuccessCriteria)
            {
                healthScore += SuccessCriteriaCompletionRate / 100.0;
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
    }
}