using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.Learning.Enums;
using NouAssist.LearningDevelopment.Learning.Aggregates;
using NouAssist.LearningDevelopment.Learning.Entities;
using NouAssist.LearningDevelopment.Learning.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace NouAssist.LearningDevelopment.Learning.Services
{
    /// <summary>
    /// Domain service for orchestrating learning workflow operations
    /// </summary>
    public class LearningWorkflowService : DomainService
    {
        private readonly LearningProgressService _progressService;

        public LearningWorkflowService(LearningProgressService progressService)
        {
            _progressService = progressService;
        }

        public LearningSession StartNewSession(Topic topic, Guid userId)
        {
            if (topic.IsFolder)
            {
                throw new BusinessException("Learning:CannotStartSessionForFolder",
                    "Learning sessions can only be started for non-folder topics.");
            }

            if (topic.Status == TopicStatus.Completed)
            {
                throw new BusinessException("Learning:TopicAlreadyCompleted",
                    "Cannot start a new session for a completed topic.");
            }

            var session = new LearningSession(
                GuidGenerator.Create(),
                userId,
                topic.Id,
                topic.TenantId
            );

            topic.AssociateLearningSession(session.Id);
            topic.StartLearning();

            return session;
        }

        public void AdvancePhase(LearningSession session, Topic topic, int timeSpentMs)
        {
            // Validate phase completion
            var currentMilestone = session.GetCurrentPhaseMilestone();
            ValidatePhaseCompletion(currentMilestone, session.CurrentPhase);

            // Mark milestone as completed
            session.CompleteCurrentPhase(timeSpentMs);

            // Advance to next phase
            session.AdvanceToNextPhase(timeSpentMs);

            // Update topic progress
            var newProgress = _progressService.CalculateTopicProgress(topic, session);
            topic.UpdateProgress(newProgress);

            // If session completed, mark topic as completed
            if (session.Completed)
            {
                topic.Complete();
            }
        }

        public void RecordPreLearningQuestions(
            LearningSession session,
            List<string> questions)
        {
            if (session.CurrentPhase != LearningPhase.PreLearning)
            {
                throw new BusinessException("Learning:InvalidPhase",
                    "Pre-learning questions can only be added during Phase 1.");
            }

            var milestone = session.GetCurrentPhaseMilestone();
            milestone.AddPreLearningQuestions(questions);
            session.RecordActivity();
        }

        public void RecordSectionReflection(
            LearningSession session,
            string sectionKey,
            string reflection)
        {
            if (session.CurrentPhase != LearningPhase.ActiveLearning)
            {
                throw new BusinessException("Learning:InvalidPhase",
                    "Section reflections can only be added during Phase 2.");
            }

            var milestone = session.GetCurrentPhaseMilestone();
            milestone.AddSectionReflection(sectionKey, reflection);
            session.RecordActivity();
        }

        public void RecordFeynmanSummary(
            LearningSession session,
            string summary)
        {
            if (session.CurrentPhase != LearningPhase.FeynmanSummary)
            {
                throw new BusinessException("Learning:InvalidPhase",
                    "Feynman summary can only be recorded during Phase 3.");
            }

            var milestone = session.GetCurrentPhaseMilestone();
            milestone.SetCornellSummary(summary);
            session.RecordActivity();
        }

        public void RecordBloomAssessment(
            LearningSession session,
            BloomAssessmentResult assessment)
        {
            if (session.CurrentPhase != LearningPhase.BloomAssessment)
            {
                throw new BusinessException("Learning:InvalidPhase",
                    "Bloom assessment can only be recorded during Phase 4.");
            }

            var milestone = session.GetCurrentPhaseMilestone();
            milestone.SetBloomAssessment(assessment);
            session.RecordActivity();
        }

        public void CompleteSpacedRepetitionCycle(
            LearningSession session,
            int cycleNumber,
            double recallAccuracy,
            double averageConfidence,
            int completionRate,
            Dictionary<string, bool> improvements,
            Dictionary<string, string> reflection)
        {
            if (session.CurrentPhase != LearningPhase.SpacedRepetition)
            {
                throw new BusinessException("Learning:InvalidPhase",
                    "Spaced repetition cycles can only be completed during Phase 5.");
            }

            var cycle = new CycleProgress(cycleNumber);
            var completedCycle = cycle.Complete(
                recallAccuracy,
                averageConfidence,
                completionRate,
                improvements,
                reflection
            );

            var milestone = session.GetCurrentPhaseMilestone();
            milestone.AddSpacedCycle(completedCycle);
            session.IncrementReviewCycle();
            session.RecordActivity();
        }

        public void RecordMasteryValidation(
            LearningSession session,
            Dictionary<string, object> validation)
        {
            if (session.CurrentPhase != LearningPhase.Mastery)
            {
                throw new BusinessException("Learning:InvalidPhase",
                    "Mastery validation can only be recorded during Phase 6.");
            }

            var milestone = session.GetCurrentPhaseMilestone();
            milestone.SetMasteryValidation(validation);
            session.RecordActivity();
        }

        private void ValidatePhaseCompletion(PhaseMilestone milestone, LearningPhase phase)
        {
            switch (phase)
            {
                case LearningPhase.PreLearning:
                    if (milestone.Questions == null || !milestone.Questions.Any())
                    {
                        throw new BusinessException("Learning:IncompletePhase",
                            "Pre-learning phase requires at least one question.");
                    }
                    break;

                case LearningPhase.ActiveLearning:
                    if (milestone.SectionReflections == null || !milestone.SectionReflections.Any())
                    {
                        throw new BusinessException("Learning:IncompletePhase",
                            "Active learning phase requires at least one section reflection.");
                    }
                    break;

                case LearningPhase.FeynmanSummary:
                    if (string.IsNullOrWhiteSpace(milestone.CornellSummary))
                    {
                        throw new BusinessException("Learning:IncompletePhase",
                            "Feynman summary phase requires a summary.");
                    }
                    break;

                case LearningPhase.BloomAssessment:
                    if (milestone.BloomAssessment == null)
                    {
                        throw new BusinessException("Learning:IncompletePhase",
                            "Bloom assessment phase requires assessment scores.");
                    }
                    break;

                case LearningPhase.SpacedRepetition:
                    if (milestone.SpacedCycles.Count < 4)
                    {
                        throw new BusinessException("Learning:IncompletePhase",
                            "Spaced repetition phase requires 4 completed cycles.");
                    }
                    break;

                case LearningPhase.Mastery:
                    if (milestone.MasteryValidation == null || !milestone.MasteryValidation.Any())
                    {
                        throw new BusinessException("Learning:IncompletePhase",
                            "Mastery phase requires validation data.");
                    }
                    break;
            }
        }
    }
}
