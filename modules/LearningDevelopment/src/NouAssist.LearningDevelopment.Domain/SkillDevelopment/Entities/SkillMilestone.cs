using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.LearningDevelopment.SkillDevelopment.ValueObjects;
using NouAssist.SkillDevelopment.Enums;
using Volo.Abp.Domain.Entities;

namespace NouAssist.LearningDevelopment.SkillDevelopment.Entities
{
    public class SkillMilestone : Entity
    {
        public Guid SessionId { get; private set; }
        public DevelopmentStep Step { get; private set; }
        public bool Completed { get; private set; }
        public DateTime? CompletedAt { get; private set; }
        public TimeTracking TimeTracking { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        // Step-specific data
        public AssessmentResult InitialAssessment { get; private set; } // Step 1
        public List<PracticePlanItem> PracticePlan { get; private set; } // Step 2
        private readonly List<PracticeLog> _practiceLogs = new(); // Step 3
        public IReadOnlyCollection<PracticeLog> PracticeLogs => _practiceLogs.AsReadOnly();
        
        public Dictionary<string, int> ReviewFeedback { get; private set; } // Step 4
        private readonly List<ApplicationExample> _applications = new(); // Step 5
        public IReadOnlyCollection<ApplicationExample> Applications => _applications.AsReadOnly();
        
        public Dictionary<string, object> MasteryValidation { get; private set; } // Step 6

        protected SkillMilestone() 
        { 
            PracticePlan = new List<PracticePlanItem>();
            ReviewFeedback = new Dictionary<string, int>();
            MasteryValidation = new Dictionary<string, object>();
        }

        public SkillMilestone(Guid sessionId, DevelopmentStep step, DateTime startedAt)
        {
            SessionId = sessionId;
            Step = step;
            Completed = false;
            TimeTracking = new TimeTracking(startedAt);
            UpdatedAt = startedAt;
            PracticePlan = new List<PracticePlanItem>();
            ReviewFeedback = new Dictionary<string, int>();
            MasteryValidation = new Dictionary<string, object>();
        }

        // Step 1: Assessment
        public void RecordAssessment(AssessmentResult assessment)
        {
            if (Step != DevelopmentStep.Assessment)
            {
                throw new InvalidOperationException("Assessment can only be recorded in Assessment step");
            }

            InitialAssessment = assessment;
            UpdatedAt = DateTime.UtcNow;
        }

        // Step 2: Planning
        public void AddPracticePlanItem(PracticePlanItem item)
        {
            if (Step != DevelopmentStep.Planning)
            {
                throw new InvalidOperationException("Practice plan can only be added in Planning step");
            }

            PracticePlan.Add(item);
            UpdatedAt = DateTime.UtcNow;
        }

        // Step 3: Practice
        public void AddPracticeLog(PracticeLog log)
        {
            if (Step != DevelopmentStep.Practice)
            {
                throw new InvalidOperationException("Practice logs can only be added in Practice step");
            }

            _practiceLogs.Add(log);
            TimeTracking.AddTime(log.DurationMinutes);
            UpdatedAt = DateTime.UtcNow;
        }

        // Step 4: Review
        public void AddReviewFeedback(string aspect, int score)
        {
            if (Step != DevelopmentStep.Review)
            {
                throw new InvalidOperationException("Review feedback can only be added in Review step");
            }

            ReviewFeedback[aspect] = score;
            UpdatedAt = DateTime.UtcNow;
        }

        // Step 5: Application
        public void AddApplication(ApplicationExample example)
        {
            if (Step != DevelopmentStep.Application)
            {
                throw new InvalidOperationException("Applications can only be added in Application step");
            }

            _applications.Add(example);
            UpdatedAt = DateTime.UtcNow;
        }

        // Step 6: Mastery
        public void RecordMasteryValidation(Dictionary<string, object> validation)
        {
            if (Step != DevelopmentStep.Mastery)
            {
                throw new InvalidOperationException("Mastery validation can only be recorded in Mastery step");
            }

            MasteryValidation = validation;
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkCompleted(DateTime completedAt)
        {
            Completed = true;
            CompletedAt = completedAt;
            TimeTracking.Complete(completedAt);
            UpdatedAt = completedAt;
        }

        public int GetTotalPracticeMinutes() => _practiceLogs.Sum(l => l.DurationMinutes);

        public override object[] GetKeys() => new object[] { SessionId, (int)Step };
    }
}
