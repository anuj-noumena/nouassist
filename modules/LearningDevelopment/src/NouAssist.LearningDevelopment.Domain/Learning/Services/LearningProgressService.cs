using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.Learning.Aggregates;
using NouAssist.Learning.Enums;
using NouAssist.Learning.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace NouAssist.Learning.Services
{
    /// <summary>
    /// Domain service for calculating and managing learning progress
    /// </summary>
    public class LearningProgressService : DomainService
    {
        public int CalculateTopicProgress(Topic topic, LearningSession session)
        {
            if (session == null || !session.Completed)
            {
                // Calculate based on current phase
                var phaseWeight = 100.0 / 6; // Each phase worth ~16.67%
                var completedPhases = (int)session?.CurrentPhase - 1 ?? 0;
                return (int)(completedPhases * phaseWeight);
            }

            return 100; // Session completed = 100%
        }

        public int CalculateFolderProgress(Topic folder)
        {
            if (!folder.IsFolder)
            {
                throw new BusinessException("Learning:NotAFolder",
                    "Progress calculation for folders requires a folder topic.");
            }

            if (!folder.Children.Any())
            {
                return 0;
            }

            var totalProgress = folder.Children.Sum(c => c.Progress);
            return totalProgress / folder.Children.Count;
        }

        public TopicMetrics CalculateTopicMetrics(Topic topic, LearningSession session)
        {
            var totalTimeMs = session?.GetTotalTimeSpent() ?? 0;
            var estimatedTimeMs = topic.EstimatedHours * 3600000;
            var remainingTimeMs = Math.Max(0, estimatedTimeMs - totalTimeMs);

            var activeChildren = topic.Children.Count(c => c.Status == TopicStatus.Active);
            var completedChildren = topic.Children.Count(c => c.Status == TopicStatus.Completed);
            var avgProgress = topic.Children.Any()
                ? topic.Children.Average(c => c.Progress)
                : 0;

            return new TopicMetrics(
                totalTimeMs,
                remainingTimeMs,
                topic.Progress,
                activeChildren,
                completedChildren,
                avgProgress
            );
        }

        public bool ShouldTriggerSpacedRepetition(LearningSession session)
        {
            if (session.CurrentPhase != LearningPhase.SpacedRepetition)
            {
                return false;
            }

            var milestone = session.PhaseMilestones[(int)LearningPhase.SpacedRepetition];
            var completedCycles = milestone.SpacedCycles.Count(kvp => kvp.Value.Completed);

            return completedCycles < 4; // Max 4 cycles
        }

        public DateTime CalculateNextReviewDate(int cycleNumber)
        {
            // Spaced repetition intervals: 1 day, 3 days, 7 days, 14 days
            var intervals = new[] { 1, 3, 7, 14 };
            var days = cycleNumber <= intervals.Length
                ? intervals[cycleNumber - 1]
                : 30; // Default to monthly

            return Clock.Now.AddDays(days);
        }

        public double CalculateMasteryScore(LearningSession session)
        {
            if (!session.Completed)
            {
                return 0;
            }

            var scores = new List<double>();

            // Bloom Assessment score (Phase 4)
            var bloomMilestone = session.PhaseMilestones[(int)LearningPhase.BloomAssessment];
            if (bloomMilestone.BloomAssessment != null)
            {
                scores.Add(bloomMilestone.BloomAssessment.OverallScore);
            }

            // Spaced Repetition accuracy (Phase 5)
            var spacedMilestone = session.PhaseMilestones[(int)LearningPhase.SpacedRepetition];
            if (spacedMilestone.SpacedCycles.Any())
            {
                var avgAccuracy = spacedMilestone.SpacedCycles.Values
                    .Where(c => c.RecallAccuracy.HasValue)
                    .Average(c => c.RecallAccuracy.Value);
                scores.Add(avgAccuracy);
            }

            return scores.Any() ? scores.Average() : 0;
        }
    }
}
