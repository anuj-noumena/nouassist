using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.Learning.Enums;
using NouAssist.Learning.Events;
using NouAssist.LearningDevelopment.Learning.Entities;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.LearningDevelopment.Learning.Aggregates
{
    /// <summary>
    /// Aggregate root orchestrating the 6-phase learning workflow
    /// </summary>
    public class LearningSession : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid UserId { get; private set; }
        public Guid TopicId { get; private set; }
        public Guid? TenantId { get; private set; }
        public LearningPhase CurrentPhase { get; private set; }
        public int ReviewCycle { get; private set; }
        public bool Completed { get; private set; }
        public bool Paused { get; private set; }
        public DateTime? PausedAt { get; private set; }
        public DateTime? ResumedAt { get; private set; }
        public DateTime LastActivity { get; private set; }

        private readonly Dictionary<int, PhaseMilestone> _phaseMilestones = new();
        public IReadOnlyDictionary<int, PhaseMilestone> PhaseMilestones => _phaseMilestones;

        private readonly Dictionary<int, int> _timeTracking = new();
        public IReadOnlyDictionary<int, int> TimeTracking => _timeTracking;

        protected LearningSession() { /* ORM */ }

        public LearningSession(Guid id, Guid userId, Guid topicId, Guid? tenantId = null)
            : base(id)
        {
            UserId = Check.NotNull(userId, nameof(userId));
            TopicId = Check.NotNull(topicId, nameof(topicId));
            TenantId = tenantId;
            CurrentPhase = LearningPhase.PreLearning;
            ReviewCycle = 0;
            Completed = false;
            Paused = false;
            LastActivity = DateTime.UtcNow;

            // Initialize all phase milestones
            InitializePhaseMilestones();

            AddDistributedEvent(new LearningSessionStartedEto
            {
                SessionId = Id,
                TopicId = TopicId,
                UserId = UserId,
                TenantId = TenantId,
                StartedAt = CreationTime
            });
        }

        private void InitializePhaseMilestones()
        {
            for (int i = 1; i <= 6; i++)
            {
                var phase = (LearningPhase)i;
                _phaseMilestones[i] = new PhaseMilestone(Id, phase);
                _timeTracking[i] = 0;
            }
        }

        public PhaseMilestone GetCurrentPhaseMilestone()
        {
            return _phaseMilestones[(int)CurrentPhase];
        }

        public void AdvanceToNextPhase(int timeSpentMs)
        {
            if (Completed)
            {
                throw new BusinessException("Learning:SessionAlreadyCompleted");
            }

            if (Paused)
            {
                throw new BusinessException("Learning:SessionPaused");
            }

            var currentMilestone = GetCurrentPhaseMilestone();
            if (!currentMilestone.Completed)
            {
                throw new BusinessException("Learning:PhaseNotCompleted",
                    $"Phase {CurrentPhase} must be completed before advancing.");
            }

            // Track time for current phase
            _timeTracking[(int)CurrentPhase] += timeSpentMs;

            // Emit phase completed event
            AddDistributedEvent(new PhaseCompletedEto
            {
                SessionId = Id,
                TopicId = TopicId,
                UserId = UserId,
                TenantId = TenantId,
                Phase = CurrentPhase,
                TimeSpentMs = _timeTracking[(int)CurrentPhase],
                CompletedAt = DateTime.UtcNow
            });

            // Advance phase
            if (CurrentPhase == LearningPhase.Mastery)
            {
                CompleteSession();
            }
            else
            {
                CurrentPhase = (LearningPhase)((int)CurrentPhase + 1);
                LastActivity = DateTime.UtcNow;
            }
        }

        public void CompleteCurrentPhase(int timeSpentMs)
        {
            var milestone = GetCurrentPhaseMilestone();
            milestone.MarkCompleted(timeSpentMs);
            LastActivity = DateTime.UtcNow;
        }

        private void CompleteSession()
        {
            Completed = true;
            LastActivity = DateTime.UtcNow;

            var totalTime = _timeTracking.Values.Sum();

            AddDistributedEvent(new TopicMasteredEto
            {
                TopicId = TopicId,
                UserId = UserId,
                TenantId = TenantId,
                SessionId = Id,
                TotalTimeSpentMs = totalTime,
                MasteredAt = DateTime.UtcNow
            });
        }

        public void PauseSession()
        {
            if (Completed)
            {
                throw new BusinessException("Learning:CannotPauseCompletedSession");
            }

            if (!Paused)
            {
                Paused = true;
                PausedAt = DateTime.UtcNow;
                LastActivity = DateTime.UtcNow;
            }
        }

        public void ResumeSession()
        {
            if (!Paused)
            {
                throw new BusinessException("Learning:SessionNotPaused");
            }

            Paused = false;
            ResumedAt = DateTime.UtcNow;
            LastActivity = DateTime.UtcNow;
        }

        public void IncrementReviewCycle()
        {
            if (CurrentPhase == LearningPhase.SpacedRepetition)
            {
                ReviewCycle++;
                LastActivity = DateTime.UtcNow;
            }
        }

        public void RecordActivity()
        {
            LastActivity = DateTime.UtcNow;
        }

        public int GetTotalTimeSpent()
        {
            return _timeTracking.Values.Sum();
        }

        public int GetPhaseTimeSpent(LearningPhase phase)
        {
            return _timeTracking.TryGetValue((int)phase, out var time) ? time : 0;
        }
    }
}
