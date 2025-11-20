using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using NouAssist.FinancialManagement.FinancialPlanning.Enums;
using Volo.Abp.Domain.Services;

namespace NouAssist.FinancialManagement.FinancialPlanning.Services
{
    public class GoalProgressCalculationService : DomainService
    {
        public GoalProgress CalculateProgress(FinancialGoal goal)
        {
            var currentProgress = (goal.CurrentAmount.Amount / goal.TargetAmount.Amount) * 100;
            var daysRemaining = Math.Max(0, (goal.TargetDate - Clock.Now).Days);
            var totalDays = Math.Max(1, (goal.TargetDate - goal.CreationTime).Days);
            var daysElapsed = totalDays - daysRemaining;
            var timeProgress = ((decimal)daysElapsed / totalDays) * 100;

            var remainingAmount = Math.Max(0, goal.TargetAmount.Amount - goal.CurrentAmount.Amount);
            var projectedCompletionDate = CalculateProjectedCompletionDate(goal);
            var isOnTrack = currentProgress >= timeProgress;
            var requiredMonthlyContribution = CalculateRequiredMonthlyContribution(goal);

            return new GoalProgress(
                goal.Id,
                goal.Name,
                goal.TargetAmount.Amount,
                goal.CurrentAmount.Amount,
                remainingAmount,
                currentProgress,
                timeProgress,
                daysRemaining,
                isOnTrack,
                projectedCompletionDate,
                requiredMonthlyContribution
            );
        }

        public DateTime? CalculateProjectedCompletionDate(FinancialGoal goal)
        {
            if (goal.CurrentAmount.Amount == 0)
                return null;

            var totalDays = (Clock.Now - goal.CreationTime).Days;
            if (totalDays <= 0)
                return null;

            var averageDailyContribution = goal.CurrentAmount.Amount / totalDays;
            if (averageDailyContribution <= 0)
                return null;

            var remainingAmount = goal.TargetAmount.Amount - goal.CurrentAmount.Amount;
            var daysToCompletion = (int)Math.Ceiling(remainingAmount / averageDailyContribution);

            return Clock.Now.AddDays(daysToCompletion);
        }

        public decimal CalculateRequiredMonthlyContribution(FinancialGoal goal)
        {
            var remainingAmount = Math.Max(0, goal.TargetAmount.Amount - goal.CurrentAmount.Amount);
            var monthsRemaining = Math.Max(1, (goal.TargetDate - Clock.Now).Days / 30.0m);

            return remainingAmount / monthsRemaining;
        }

        public List<FinancialGoal> GetGoalsAtRisk(List<FinancialGoal> goals, decimal riskThresholdPercent = 10)
        {
            return goals
                .Where(g => g.Status == GoalStatus.Active)
                .Where(g =>
                {
                    var progress = CalculateProgress(g);
                    return !progress.IsOnTrack && (progress.TimeProgress - progress.CurrentProgress) >= riskThresholdPercent;
                })
                .ToList();
        }

        public GoalRecommendation GenerateRecommendation(FinancialGoal goal)
        {
            var progress = CalculateProgress(goal);

            if (progress.CurrentProgress >= 100)
            {
                return new GoalRecommendation(
                    goal.Id,
                    RecommendationType.Complete,
                    "Congratulations! You've reached your goal. Consider marking it as complete."
                );
            }

            if (!progress.IsOnTrack)
            {
                var additionalRequired = progress.RequiredMonthlyContribution - (goal.CurrentAmount.Amount / Math.Max(1, (Clock.Now - goal.CreationTime).Days / 30.0m));
                return new GoalRecommendation(
                    goal.Id,
                    RecommendationType.IncreaseContributions,
                    $"Goal is behind schedule. Consider increasing monthly contributions by {additionalRequired:C} to stay on track."
                );
            }

            if (progress.DaysRemaining <= 30)
            {
                return new GoalRecommendation(
                    goal.Id,
                    RecommendationType.NearDeadline,
                    $"Goal deadline is approaching in {progress.DaysRemaining} days. You need {progress.RemainingAmount:C} more."
                );
            }

            return new GoalRecommendation(
                goal.Id,
                RecommendationType.OnTrack,
                $"Great progress! Continue contributing approximately {progress.RequiredMonthlyContribution:C} monthly."
            );
        }

        public List<MilestoneAlert> GenerateMilestoneAlerts(List<FinancialGoal> goals)
        {
            var alerts = new List<MilestoneAlert>();

            foreach (var goal in goals.Where(g => g.Status == GoalStatus.Active))
            {
                var progress = CalculateProgress(goal);

                // Check for milestone achievements
                var milestones = new[] { 25m, 50m, 75m, 90m };
                foreach (var milestone in milestones)
                {
                    if (progress.CurrentProgress >= milestone && progress.CurrentProgress < (milestone + 5))
                    {
                        alerts.Add(new MilestoneAlert(
                            goal.Id,
                            goal.Name,
                            milestone,
                            $"You've reached {milestone}% of your goal!"
                        ));
                    }
                }
            }

            return alerts;
        }
    }

    public class GoalProgress
    {
        public Guid GoalId { get; }
        public string GoalName { get; }
        public decimal TargetAmount { get; }
        public decimal CurrentAmount { get; }
        public decimal RemainingAmount { get; }
        public decimal CurrentProgress { get; }
        public decimal TimeProgress { get; }
        public int DaysRemaining { get; }
        public bool IsOnTrack { get; }
        public DateTime? ProjectedCompletionDate { get; }
        public decimal RequiredMonthlyContribution { get; }

        public GoalProgress(
            Guid goalId,
            string goalName,
            decimal targetAmount,
            decimal currentAmount,
            decimal remainingAmount,
            decimal currentProgress,
            decimal timeProgress,
            int daysRemaining,
            bool isOnTrack,
            DateTime? projectedCompletionDate,
            decimal requiredMonthlyContribution)
        {
            GoalId = goalId;
            GoalName = goalName;
            TargetAmount = targetAmount;
            CurrentAmount = currentAmount;
            RemainingAmount = remainingAmount;
            CurrentProgress = currentProgress;
            TimeProgress = timeProgress;
            DaysRemaining = daysRemaining;
            IsOnTrack = isOnTrack;
            ProjectedCompletionDate = projectedCompletionDate;
            RequiredMonthlyContribution = requiredMonthlyContribution;
        }
    }

    public class GoalRecommendation
    {
        public Guid GoalId { get; }
        public RecommendationType Type { get; }
        public string Message { get; }

        public GoalRecommendation(Guid goalId, RecommendationType type, string message)
        {
            GoalId = goalId;
            Type = type;
            Message = message;
        }
    }

    public enum RecommendationType
    {
        OnTrack = 1,
        IncreaseContributions = 2,
        NearDeadline = 3,
        Complete = 4
    }

    public class MilestoneAlert
    {
        public Guid GoalId { get; }
        public string GoalName { get; }
        public decimal MilestonePercent { get; }
        public string Message { get; }

        public MilestoneAlert(Guid goalId, string goalName, decimal milestonePercent, string message)
        {
            GoalId = goalId;
            GoalName = goalName;
            MilestonePercent = milestonePercent;
            Message = message;
        }
    }
}
