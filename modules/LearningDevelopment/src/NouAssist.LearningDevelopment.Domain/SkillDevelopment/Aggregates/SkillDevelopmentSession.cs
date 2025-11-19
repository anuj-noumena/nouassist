using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.LearningDevelopment.SkillDevelopment.Entities;
using NouAssist.SkillDevelopment.Enums;
using NouAssist.SkillDevelopment.Events;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.LearningDevelopment.SkillDevelopment.Aggregates
{
    public class SkillDevelopmentSession : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid UserId { get; private set; }
        public Guid SkillId { get; private set; }
        public Guid? TenantId { get; private set; }
        public DevelopmentStep CurrentStep { get; private set; }
        public int ReviewCycle { get; private set; }
        public bool Completed { get; private set; }
        public bool Paused { get; private set; }
        public DateTime? PausedAt { get; private set; }
        public DateTime? ResumedAt { get; private set; }
        public DateTime LastActivity { get; private set; }
        public int? OverallProficiencyGain { get; private set; }

        private readonly Dictionary<DevelopmentStep, SkillMilestone> _milestones = new();
        public IReadOnlyDictionary<DevelopmentStep, SkillMilestone> Milestones => _milestones;

        private readonly Dictionary<DevelopmentStep, int> _practiceTimeTracking = new();
        public IReadOnlyDictionary<DevelopmentStep, int> PracticeTimeTracking => _practiceTimeTracking;

        protected SkillDevelopmentSession() { /* ORM */ }

        public SkillDevelopmentSession(
            Guid id,
            Guid userId,
            Guid skillId,
            Guid? tenantId = null)
            : base(id)
        {
            UserId = userId;
            SkillId = skillId;
            TenantId = tenantId;
            CurrentStep = DevelopmentStep.Assessment;
            ReviewCycle = 0;
            Completed = false;
            Paused = false;
            LastActivity = DateTime.UtcNow;

            // Initialize first milestone
            _milestones[DevelopmentStep.Assessment] = new SkillMilestone(Id, DevelopmentStep.Assessment, DateTime.UtcNow);
        }

        public SkillMilestone GetCurrentMilestone()
        {
            return _milestones.GetValueOrDefault(CurrentStep);
        }

        public void AdvanceToNextStep()
        {
            if (Completed)
            {
                throw new BusinessException("SkillDevelopment:SessionAlreadyCompleted");
            }

            if (Paused)
            {
                throw new BusinessException("SkillDevelopment:SessionIsPaused");
            }

            var currentMilestone = GetCurrentMilestone();
            if (currentMilestone == null || !currentMilestone.Completed)
            {
                throw new BusinessException("SkillDevelopment:CurrentStepNotCompleted");
            }

            // Track time for completed step
            _practiceTimeTracking[CurrentStep] = currentMilestone.TimeTracking.TotalMinutes;

            // Publish step completion event
            AddDistributedEvent(new DevelopmentStepCompletedEto
            {
                SessionId = Id,
                SkillId = SkillId,
                UserId = UserId,
                TenantId = TenantId,
                CompletedStep = CurrentStep,
                CompletedAt = DateTime.UtcNow,
                TimeSpentMinutes = currentMilestone.TimeTracking.TotalMinutes
            });

            // Check if we've completed all steps
            if (CurrentStep == DevelopmentStep.Mastery)
            {
                CompleteSession();
                return;
            }

            // Move to next step
            CurrentStep = (DevelopmentStep)((int)CurrentStep + 1);
            _milestones[CurrentStep] = new SkillMilestone(Id, CurrentStep, DateTime.UtcNow);
            LastActivity = DateTime.UtcNow;
        }

        public void CompleteCurrentStep()
        {
            var milestone = GetCurrentMilestone();
            if (milestone == null)
            {
                throw new BusinessException("SkillDevelopment:NoActiveMilestone");
            }

            milestone.MarkCompleted(DateTime.UtcNow);
            LastActivity = DateTime.UtcNow;
        }

        public void Pause()
        {
            if (Completed)
            {
                throw new BusinessException("SkillDevelopment:CannotPauseCompletedSession");
            }

            if (Paused)
            {
                throw new BusinessException("SkillDevelopment:SessionAlreadyPaused");
            }

            Paused = true;
            PausedAt = DateTime.UtcNow;
        }

        public void Resume()
        {
            if (!Paused)
            {
                throw new BusinessException("SkillDevelopment:SessionNotPaused");
            }

            Paused = false;
            ResumedAt = DateTime.UtcNow;
            LastActivity = DateTime.UtcNow;
        }

        public void StartReviewCycle()
        {
            if (!Completed)
            {
                throw new BusinessException("SkillDevelopment:CannotReviewIncompleteSession");
            }

            ReviewCycle++;
            CurrentStep = DevelopmentStep.Review;
            _milestones[CurrentStep] = new SkillMilestone(Id, CurrentStep, DateTime.UtcNow);
            LastActivity = DateTime.UtcNow;
        }

        private void CompleteSession()
        {
            Completed = true;
            LastActivity = DateTime.UtcNow;
            
            // Calculate overall proficiency gain
            OverallProficiencyGain = CalculateProficiencyGain();
        }

        private int CalculateProficiencyGain()
        {
            // Calculate based on completed milestones and practice time
            var totalPracticeMinutes = _practiceTimeTracking.Values.Sum();
            var completedSteps = _milestones.Count(m => m.Value.Completed);
            
            // Base gain on completion and practice time
            var baseGain = completedSteps * 10;
            var practiceBonus = Math.Min(20, totalPracticeMinutes / 60); // Max 20 points from practice
            
            return Math.Min(100, baseGain + practiceBonus);
        }

        public int GetTotalPracticeMinutes() =>
            _milestones.Values
                .SelectMany(m => m.PracticeLogs)
                .Sum(l => l.DurationMinutes);

        public int GetTotalApplicationsCount() =>
            _milestones.Values.Sum(m => m.Applications.Count);
    }
}
