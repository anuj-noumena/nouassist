using System;
using System.Collections.Generic;

namespace NouAssist.SharedKernel.Aggregates;

/// <summary>
/// Aggregate for calculating user productivity metrics across all domains
/// NOTE: This references external bounded contexts (Tasks, Projects, Habits, Calendar, Reviews)
/// which need to be implemented separately
/// </summary>
public class UserProductivityAggregate
{
    public Guid UserId { get; private set; }
    public DateTime PeriodStart { get; private set; }
    public DateTime PeriodEnd { get; private set; }

    // Task Metrics (from Tasks bounded context - to be implemented)
    public int TotalTasks { get; set; }
    public int CompletedTasks { get; set; }
    public int OverdueTasks { get; set; }
    public int TotalTimeTrackedMinutes { get; set; }

    // Habit Metrics (from Habits bounded context - to be implemented)
    public int TotalHabitEntries { get; set; }
    public int CompletedHabitEntries { get; set; }

    // Goal Metrics (already implemented)
    public int TotalGoals { get; set; }
    public int AchievedGoals { get; set; }
    public int ActiveGoals { get; set; }

    // Time Entry Metrics (from Calendar bounded context - to be implemented)
    public int TotalTimeEntries { get; set; }
    public int TotalFocusMinutes { get; set; }

    // Review Metrics (from Reviews bounded context - to be implemented)
    public int TotalReviews { get; set; }
    public int CompletedReviews { get; set; }

    public UserProductivityAggregate(
        Guid userId,
        DateTime periodStart,
        DateTime periodEnd)
    {
        UserId = userId;
        PeriodStart = periodStart;
        PeriodEnd = periodEnd;
    }

    public TimeSpan PeriodDuration => PeriodEnd - PeriodStart;
    public int PeriodDays => PeriodDuration.Days;

    // Task Metrics
    public double TaskCompletionRate => 
        TotalTasks == 0 ? 0.0 : (double)CompletedTasks / TotalTasks;

    public double AverageTasksPerDay => 
        PeriodDays > 0 ? (double)TotalTasks / PeriodDays : 0;

    public double AverageTimePerTask =>
        CompletedTasks == 0 ? 0.0 : (double)TotalTimeTrackedMinutes / CompletedTasks;

    // Habit Metrics
    public double HabitCompletionRate =>
        TotalHabitEntries == 0 ? 0.0 : (double)CompletedHabitEntries / TotalHabitEntries;

    // Goal Metrics
    public double AverageGoalProgress
    {
        get
        {
            // Would need to calculate from actual goal progress values
            return 0.0; // Placeholder
        }
    }

    // Time Entry Metrics
    public double AverageFocusTimePerDay =>
        PeriodDays > 0 ? (double)TotalFocusMinutes / PeriodDays : 0;

    public double FocusTimeHours => TotalFocusMinutes / 60.0;

    // Review Metrics
    public double AverageReviewCompleteness
    {
        get
        {
            // Would need actual completeness scores
            return TotalReviews == 0 ? 0.0 : (double)CompletedReviews / TotalReviews * 100;
        }
    }

    // Overall Productivity Score (0-100)
    public int ProductivityScore
    {
        get
        {
            double score = 0.0;
            int factors = 0;

            // Task completion (25%)
            if (TotalTasks > 0)
            {
                score += TaskCompletionRate * 25;
                factors++;
            }

            // Habit consistency (25%)
            if (TotalHabitEntries > 0)
            {
                score += HabitCompletionRate * 25;
                factors++;
            }

            // Goal progress (20%)
            if (TotalGoals > 0)
            {
                score += (AverageGoalProgress / 100) * 20;
                factors++;
            }

            // Review completion (15%)
            if (TotalReviews > 0)
            {
                score += ((double)CompletedReviews / TotalReviews) * 15;
                factors++;
            }

            // Time tracking (15%)
            if (TotalTasks > 0)
            {
                var trackedRate = TotalTimeTrackedMinutes > 0 ? 1.0 : 0.0;
                score += trackedRate * 15;
                factors++;
            }

            return (int)Double.Clamp(Math.Round(score), 0, 100);
        }
    }

    public ProductivityLevel GetProductivityLevel()
    {
        var score = ProductivityScore;
        return score switch
        {
            >= 90 => ProductivityLevel.Exceptional,
            >= 75 => ProductivityLevel.High,
            >= 60 => ProductivityLevel.Good,
            >= 40 => ProductivityLevel.Moderate,
            >= 20 => ProductivityLevel.Low,
            _ => ProductivityLevel.VeryLow
        };
    }

    public Dictionary<string, object> GetMetrics()
    {
        return new Dictionary<string, object>
        {
            ["period"] = new
            {
                start = PeriodStart,
                end = PeriodEnd,
                days = PeriodDays
            },
            ["tasks"] = new
            {
                total = TotalTasks,
                completed = CompletedTasks,
                overdue = OverdueTasks,
                completionRate = TaskCompletionRate,
                averagePerDay = AverageTasksPerDay,
                totalTimeMinutes = TotalTimeTrackedMinutes,
                averageTimePerTask = AverageTimePerTask
            },
            ["habits"] = new
            {
                totalEntries = TotalHabitEntries,
                completed = CompletedHabitEntries,
                completionRate = HabitCompletionRate
            },
            ["goals"] = new
            {
                total = TotalGoals,
                achieved = AchievedGoals,
                active = ActiveGoals,
                averageProgress = AverageGoalProgress
            },
            ["timeTracking"] = new
            {
                totalEntries = TotalTimeEntries,
                totalMinutes = TotalFocusMinutes,
                totalHours = FocusTimeHours,
                averagePerDay = AverageFocusTimePerDay
            },
            ["reviews"] = new
            {
                total = TotalReviews,
                completed = CompletedReviews,
                averageCompleteness = AverageReviewCompleteness
            },
            ["overall"] = new
            {
                productivityScore = ProductivityScore,
                productivityLevel = GetProductivityLevel().ToString()
            }
        };
    }
}

public enum ProductivityLevel
{
    VeryLow,
    Low,
    Moderate,
    Good,
    High,
    Exceptional
}

// Extension method for Clamp
public static class IntExtensions
{
    public static int Clamp(this int value, int min, int max)
    {
        if (value < min)
        {
            return min;
        }

        if (value > max)
        {
            return max;
        }

        return value;
    }
}