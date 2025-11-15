using System;
using System.Collections.Generic;
using NouAssist.Learning.Enums;
using NouAssist.Learning.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.Learning.Entities
{
    /// <summary>
    /// Represents progress checkpoint for a specific learning phase
    /// </summary>
    public class PhaseMilestone : Entity
    {
        public Guid SessionId { get; private set; }
        public LearningPhase Phase { get; private set; }
        public bool Completed { get; private set; }
        public DateTime? CompletedAt { get; private set; }
        public int? TimeSpentMs { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        // Phase 1: Pre-learning
        public List<string> Questions { get; private set; }

        // Phase 2: Active Learning
        public Dictionary<string, string> SectionReflections { get; private set; }

        // Phase 3: Feynman Summary
        public string CornellSummary { get; private set; }

        // Phase 4: Bloom Assessment
        public BloomAssessmentResult BloomAssessment { get; private set; }

        // Phase 5: Spaced Repetition
        public Dictionary<int, CycleProgress> SpacedCycles { get; private set; }

        // Phase 6: Mastery
        public Dictionary<string, object> MasteryValidation { get; private set; }

        protected PhaseMilestone() { /* ORM */ }

        public PhaseMilestone(Guid sessionId, LearningPhase phase)
        {
            SessionId = sessionId;
            Phase = phase;
            Completed = false;
            UpdatedAt = Clock.Now;
            Questions = new List<string>();
            SectionReflections = new Dictionary<string, string>();
            SpacedCycles = new Dictionary<int, CycleProgress>();
            MasteryValidation = new Dictionary<string, object>();
        }

        public void AddPreLearningQuestions(List<string> questions)
        {
            if (Phase != LearningPhase.PreLearning)
            {
                throw new BusinessException("Learning:InvalidPhase",
                    $"This operation is only valid for Pre-learning phase.");
            }
            Questions.AddRange(questions);
            UpdatedAt = Clock.Now;
        }

        public void AddSectionReflection(string sectionKey, string reflection)
        {
            if (Phase != LearningPhase.ActiveLearning)
            {
                throw new BusinessException("Learning:InvalidPhase",
                    $"This operation is only valid for Active Learning phase.");
            }
            SectionReflections[sectionKey] = Check.NotNullOrWhiteSpace(reflection, nameof(reflection));
            UpdatedAt = Clock.Now;
        }

        public void SetCornellSummary(string summary)
        {
            if (Phase != LearningPhase.FeynmanSummary)
            {
                throw new BusinessException("Learning:InvalidPhase",
                    $"This operation is only valid for Feynman Summary phase.");
            }
            CornellSummary = Check.NotNullOrWhiteSpace(summary, nameof(summary));
            UpdatedAt = Clock.Now;
        }

        public void SetBloomAssessment(BloomAssessmentResult assessment)
        {
            if (Phase != LearningPhase.BloomAssessment)
            {
                throw new BusinessException("Learning:InvalidPhase",
                    $"This operation is only valid for Bloom Assessment phase.");
            }
            BloomAssessment = Check.NotNull(assessment, nameof(assessment));
            UpdatedAt = Clock.Now;
        }

        public void AddSpacedCycle(CycleProgress cycle)
        {
            if (Phase != LearningPhase.SpacedRepetition)
            {
                throw new BusinessException("Learning:InvalidPhase",
                    $"This operation is only valid for Spaced Repetition phase.");
            }
            if (SpacedCycles.ContainsKey(cycle.CycleNumber))
            {
                throw new BusinessException("Learning:CycleAlreadyCompleted",
                    $"Cycle {cycle.CycleNumber} is already completed.");
            }
            SpacedCycles[cycle.CycleNumber] = cycle;
            UpdatedAt = Clock.Now;
        }

        public void SetMasteryValidation(Dictionary<string, object> validation)
        {
            if (Phase != LearningPhase.Mastery)
            {
                throw new BusinessException("Learning:InvalidPhase",
                    $"This operation is only valid for Mastery phase.");
            }
            MasteryValidation = validation ?? new Dictionary<string, object>();
            UpdatedAt = Clock.Now;
        }

        public void MarkCompleted(int timeSpentMs)
        {
            Completed = true;
            CompletedAt = Clock.Now;
            TimeSpentMs = timeSpentMs;
            UpdatedAt = Clock.Now;
        }

        public override object[] GetKeys() => new object[] { SessionId, (int)Phase };
    }
}
