using System;
using System.Collections.Generic;
using NouAssist.Learning.Enums;
using NouAssist.LearningDevelopment.Integration.Enums;
using NouAssist.SkillDevelopment.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.LearningDevelopment.Integration.Aggregates
{
    /// <summary>
    /// Aggregate root orchestrating the integration between Learning (theory) and SkillDevelopment (practice)
    /// Provides a unified learning journey from conceptual understanding to practical mastery
    /// </summary>
    public class LearningPath : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid UserId { get; private set; }
        public Guid? TenantId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public LearningPathStatus Status { get; private set; }
        public LearningPathPhase CurrentPhase { get; private set; }

        // Theory component
        public Guid TopicId { get; private set; }
        public Guid? LearningSessionId { get; private set; }
        public LearningPhase? CurrentLearningPhase { get; private set; }
        public bool TheoryCompleted { get; private set; }
        public DateTime? TheoryCompletedAt { get; private set; }

        // Practice component
        public Guid SkillId { get; private set; }
        public Guid? SkillDevelopmentSessionId { get; private set; }
        public DevelopmentStep? CurrentDevelopmentStep { get; private set; }
        public bool PracticeCompleted { get; private set; }
        public DateTime? PracticeCompletedAt { get; private set; }

        // Integration metrics
        public int TheoryProgressPercentage { get; private set; }
        public int PracticeProgressPercentage { get; private set; }
        public int OverallProgressPercentage { get; private set; }
        public int IntegratedMasteryScore { get; private set; }
        public DateTime? CompletedAt { get; private set; }
        public DateTime LastActivity { get; private set; }

        private readonly List<string> _milestones = new();
        public IReadOnlyCollection<string> Milestones => _milestones.AsReadOnly();

        protected LearningPath() { /* ORM */ }

        public LearningPath(
            Guid id,
            Guid userId,
            string title,
            Guid topicId,
            Guid skillId,
            Guid? tenantId = null)
            : base(id)
        {
            UserId = Check.NotNull(userId, nameof(userId));
            Title = Check.NotNullOrWhiteSpace(title, nameof(title), maxLength: 300);
            TopicId = Check.NotNull(topicId, nameof(topicId));
            SkillId = Check.NotNull(skillId, nameof(skillId));
            TenantId = tenantId;

            Status = LearningPathStatus.Draft;
            CurrentPhase = LearningPathPhase.Planning;
            TheoryCompleted = false;
            PracticeCompleted = false;
            TheoryProgressPercentage = 0;
            PracticeProgressPercentage = 0;
            OverallProgressPercentage = 0;
            IntegratedMasteryScore = 0;
            LastActivity = DateTime.UtcNow;
        }

        public void UpdateDescription(string description)
        {
            Description = description;
            RecordActivity();
        }

        public void Start()
        {
            if (Status != LearningPathStatus.Draft)
            {
                throw new BusinessException("Integration:CannotStartNonDraftPath");
            }

            Status = LearningPathStatus.Active;
            CurrentPhase = LearningPathPhase.TheoryLearning;
            AddMilestone("Learning path started");
            RecordActivity();
        }

        public void AssociateLearningSession(Guid sessionId)
        {
            LearningSessionId = sessionId;
            RecordActivity();
        }

        public void AssociateSkillDevelopmentSession(Guid sessionId)
        {
            SkillDevelopmentSessionId = sessionId;
            RecordActivity();
        }

        public void UpdateTheoryProgress(LearningPhase learningPhase, int progressPercentage)
        {
            if (CurrentPhase != LearningPathPhase.TheoryLearning &&
                CurrentPhase != LearningPathPhase.Transition)
            {
                throw new BusinessException("Integration:NotInTheoryPhase");
            }

            CurrentLearningPhase = learningPhase;
            TheoryProgressPercentage = progressPercentage;
            CalculateOverallProgress();
            RecordActivity();

            // Transition when BloomAssessment is reached
            if (learningPhase == LearningPhase.BloomAssessment && progressPercentage >= 70)
            {
                AddMilestone($"Bloom Assessment completed with {progressPercentage}% mastery");
                TransitionToPractice();
            }
        }

        public void CompleteTheoryPhase()
        {
            if (TheoryCompleted)
            {
                throw new BusinessException("Integration:TheoryAlreadyCompleted");
            }

            TheoryCompleted = true;
            TheoryCompletedAt = DateTime.UtcNow;
            TheoryProgressPercentage = 100;
            AddMilestone("Theory learning completed");

            // Auto-transition to practice if not already there
            if (CurrentPhase == LearningPathPhase.TheoryLearning)
            {
                TransitionToPractice();
            }

            CalculateOverallProgress();
            RecordActivity();
        }

        private void TransitionToPractice()
        {
            CurrentPhase = LearningPathPhase.Transition;
            AddMilestone("Transitioning from theory to practice");

            // Auto-advance to practical application if ready
            if (SkillDevelopmentSessionId.HasValue)
            {
                CurrentPhase = LearningPathPhase.PracticalApplication;
                AddMilestone("Started practical skill development");
            }

            RecordActivity();
        }

        public void UpdatePracticeProgress(DevelopmentStep developmentStep, int progressPercentage)
        {
            if (CurrentPhase != LearningPathPhase.PracticalApplication &&
                CurrentPhase != LearningPathPhase.Transition)
            {
                throw new BusinessException("Integration:NotInPracticePhase");
            }

            CurrentDevelopmentStep = developmentStep;
            PracticeProgressPercentage = progressPercentage;

            // Ensure we're in practical application phase once practice starts
            if (CurrentPhase == LearningPathPhase.Transition)
            {
                CurrentPhase = LearningPathPhase.PracticalApplication;
            }

            CalculateOverallProgress();
            RecordActivity();

            // Track significant milestones
            if (developmentStep == DevelopmentStep.Application && progressPercentage >= 80)
            {
                AddMilestone("Real-world application demonstrated");
            }
        }

        public void CompletePracticePhase()
        {
            if (PracticeCompleted)
            {
                throw new BusinessException("Integration:PracticeAlreadyCompleted");
            }

            PracticeCompleted = true;
            PracticeCompletedAt = DateTime.UtcNow;
            PracticeProgressPercentage = 100;
            AddMilestone("Practical skill development completed");

            CalculateOverallProgress();
            CheckForCompletion();
            RecordActivity();
        }

        private void CheckForCompletion()
        {
            if (TheoryCompleted && PracticeCompleted)
            {
                CurrentPhase = LearningPathPhase.IntegratedMastery;
                Status = LearningPathStatus.Completed;
                CompletedAt = DateTime.UtcNow;

                // Calculate integrated mastery score (weighted average)
                IntegratedMasteryScore = (int)((TheoryProgressPercentage * 0.4) + (PracticeProgressPercentage * 0.6));

                AddMilestone($"Integrated mastery achieved! Score: {IntegratedMasteryScore}%");
            }
        }

        public void EnterMaintenancePhase()
        {
            if (Status != LearningPathStatus.Completed)
            {
                throw new BusinessException("Integration:CannotEnterMaintenanceWithoutCompletion");
            }

            CurrentPhase = LearningPathPhase.Maintenance;
            AddMilestone("Entered maintenance phase for spaced repetition and ongoing practice");
            RecordActivity();
        }

        public void PutOnHold()
        {
            if (Status == LearningPathStatus.Completed || Status == LearningPathStatus.Abandoned)
            {
                throw new BusinessException("Integration:CannotPutCompletedOrAbandonedPathOnHold");
            }

            Status = LearningPathStatus.OnHold;
            AddMilestone("Learning path put on hold");
            RecordActivity();
        }

        public void Resume()
        {
            if (Status != LearningPathStatus.OnHold)
            {
                throw new BusinessException("Integration:CannotResumeNonHoldPath");
            }

            Status = LearningPathStatus.Active;
            AddMilestone("Learning path resumed");
            RecordActivity();
        }

        public void Abandon(string reason = null)
        {
            if (Status == LearningPathStatus.Completed)
            {
                throw new BusinessException("Integration:CannotAbandonCompletedPath");
            }

            Status = LearningPathStatus.Abandoned;
            var milestone = string.IsNullOrWhiteSpace(reason)
                ? "Learning path abandoned"
                : $"Learning path abandoned: {reason}";
            AddMilestone(milestone);
            RecordActivity();
        }

        private void CalculateOverallProgress()
        {
            // Weight: 40% theory, 60% practice
            OverallProgressPercentage = (int)((TheoryProgressPercentage * 0.4) + (PracticeProgressPercentage * 0.6));
        }

        private void AddMilestone(string milestone)
        {
            var timestamped = $"{DateTime.UtcNow:yyyy-MM-dd HH:mm} - {milestone}";
            _milestones.Add(timestamped);
        }

        private void RecordActivity()
        {
            LastActivity = DateTime.UtcNow;
        }

        public bool IsReadyForPractice()
        {
            return TheoryCompleted ||
                   (CurrentLearningPhase == LearningPhase.BloomAssessment && TheoryProgressPercentage >= 70);
        }

        public bool IsInTheoryPhase()
        {
            return CurrentPhase == LearningPathPhase.TheoryLearning ||
                   CurrentPhase == LearningPathPhase.Planning;
        }

        public bool IsInPracticePhase()
        {
            return CurrentPhase == LearningPathPhase.PracticalApplication ||
                   CurrentPhase == LearningPathPhase.Transition;
        }
    }
}
