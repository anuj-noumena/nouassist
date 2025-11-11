using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.PersonalDevelopment.Goals.Entities;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.PersonalDevelopment.Goals.Aggregates
{
    public class Goal : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid OwnerId { get; private set; }
        public Guid? TenantId { get; private set; }
        public string Title { get; private set; }
        public string? Description { get; set; }
        public Guid? AreaId { get; set; }
        public Guid? ParentGoalId { get; set; }
        public GoalStatus Status { get; private set; }
        public int Progress { get; private set; }
        public string? SmartDescription { get; set; }
        public string? CompletionNotes { get; private set; }
        public string? ArchiveReason { get; private set; }
        public ImportanceScore Importance { get; private set; }
        public UrgencyScore Urgency { get; private set; }
        public DateTime? TargetDate { get; set; }
        public DateTime? CompletedAt { get; private set; }

        private readonly List<GoalMilestone> _milestones = new();
        public IReadOnlyCollection<GoalMilestone> Milestones => _milestones.AsReadOnly();

        protected Goal() { }

        public Goal(
            Guid id, 
            Guid ownerId, 
            string title, 
            ImportanceScore? importance = null,
            UrgencyScore? urgency = null,
            Guid? tenantId = null) : base(id)
        {
            OwnerId = ownerId;
            Title = Check.NotNullOrWhiteSpace(title, nameof(title), maxLength: 255);
            TenantId = tenantId;
            Status = GoalStatus.Active;
            Progress = 0;
            Importance = importance ?? ImportanceScore.High();
            Urgency = urgency ?? UrgencyScore.Low();
        }

        public void UpdateProgress(int newProgress)
        {
            if (newProgress < 0 || newProgress > 100)
            {
                throw new BusinessException("Goals:InvalidProgress", 
                    "Progress must be between 0 and 100.");
            }

            var oldProgress = Progress;
            Progress = newProgress;

            AddDistributedEvent(new GoalProgressUpdatedEto
            {
                GoalId = Id,
                UserId = OwnerId,
                TenantId = TenantId,
                OldProgress = oldProgress,
                NewProgress = newProgress,
                UpdatedAt = DateTime.UtcNow
            });

            if (newProgress >= 100 && Status == GoalStatus.Active)
            {
                Achieve();
            }
        }

        public void Achieve(string? notes = null)
        {
            if (Status == GoalStatus.Achieved)
            {
                throw new BusinessException("Goals:AlreadyAchieved", 
                    "Goal is already achieved.");
            }

            Status = GoalStatus.Achieved;
            Progress = 100;
            CompletionNotes = notes;
            CompletedAt = DateTime.UtcNow;

            AddDistributedEvent(new GoalAchievedEto
            {
                GoalId = Id,
                UserId = OwnerId,
                TenantId = TenantId,
                AchievedAt = CompletedAt.Value,
                Progress = Progress,
                CompletionNotes = notes
            });
        }

        public void Pause()
        {
            if (Status != GoalStatus.Active)
            {
                throw new BusinessException("Goals:CanOnlyPauseActive", 
                    "Can only pause active goals.");
            }
            Status = GoalStatus.OnHold;
        }

        public void Resume()
        {
            if (Status != GoalStatus.OnHold)
            {
                throw new BusinessException("Goals:CanOnlyResumeOnHold", 
                    "Can only resume on-hold goals.");
            }
            Status = GoalStatus.Active;
        }

        public void Archive(string? reason = null)
        {
            Status = GoalStatus.Archived;
            ArchiveReason = reason;
        }

        public void AddMilestone(GoalMilestone milestone)
        {
            if (_milestones.Any(m => m.Title == milestone.Title))
            {
                throw new BusinessException("Goals:DuplicateMilestone", 
                    "A milestone with this title already exists.");
            }
            _milestones.Add(milestone);
        }

        public void RemoveMilestone(Guid milestoneId)
        {
            var milestone = _milestones.FirstOrDefault(m => m.Id == milestoneId);
            if (milestone != null)
            {
                _milestones.Remove(milestone);
            }
        }

        public void UpdateImportance(ImportanceScore importance)
        {
            Importance = importance;
        }

        public void UpdateUrgency(UrgencyScore urgency)
        {
            Urgency = urgency;
        }

        public EisenhowerQuadrant GetEisenhowerQuadrant() => 
            Importance.DetermineQuadrant(Urgency);

        public bool IsActive => Status == GoalStatus.Active;
        public bool IsOnHold => Status == GoalStatus.OnHold;
        public bool IsAchieved => Status == GoalStatus.Achieved;
        public bool IsArchived => Status == GoalStatus.Archived;
        public bool IsCompleted => Progress >= 100 || IsAchieved;
        
        public bool IsOverdue => 
            TargetDate.HasValue && 
            DateTime.UtcNow > TargetDate.Value && 
            !IsCompleted;

        public TimeSpan? GetTimeRemaining()
        {
            if (!TargetDate.HasValue || IsCompleted)
                return null;
            return TargetDate.Value - DateTime.UtcNow;
        }

        public int? GetDaysRemaining() => GetTimeRemaining()?.Days;

        public GoalProgressLevel GetProgressLevel()
        {
            if (Progress == 0) return GoalProgressLevel.NotStarted;
            if (Progress < 25) return GoalProgressLevel.JustStarted;
            if (Progress < 50) return GoalProgressLevel.Progressing;
            if (Progress < 75) return GoalProgressLevel.WellUnderway;
            if (Progress < 100) return GoalProgressLevel.NearlyComplete;
            return GoalProgressLevel.Completed;
        }

        public int GetCompletedMilestonesCount() => 
            _milestones.Count(m => m.IsCompleted);

        public double GetMilestoneCompletionRate()
        {
            if (_milestones.Count == 0) return 0;
            return (double)GetCompletedMilestonesCount() / _milestones.Count;
        }
    }
}
