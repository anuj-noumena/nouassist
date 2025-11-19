using System;
using System.Collections.Generic;
using System.Linq;

namespace NouAssist.PersonalDevelopment.Goals.Aggregates
{
    /// <summary>
    /// Rich aggregate combining Goal with related entities for comprehensive analysis
    /// </summary>
    public class GoalAggregate
    {
        public Goal Goal { get; private set; }
        public List<ProjectSummary> RelatedProjects { get; private set; }
        public List<TaskSummary> RelatedTasks { get; private set; }
        public List<UserAchievement> Achievements { get; private set; }

        public GoalAggregate(
            Goal goal,
            List<ProjectSummary>? relatedProjects = null,
            List<TaskSummary>? relatedTasks = null,
            List<UserAchievement>? achievements = null)
        {
            Goal = goal;
            RelatedProjects = relatedProjects ?? new List<ProjectSummary>();
            RelatedTasks = relatedTasks ?? new List<TaskSummary>();
            Achievements = achievements ?? new List<UserAchievement>();
        }

        // Project Analysis
        public int TotalRelatedProjects => RelatedProjects.Count;
        public int CompletedProjects => RelatedProjects.Count(p => p.IsCompleted);
        public double ProjectCompletionRate => 
            TotalRelatedProjects == 0 ? 0 : (double)CompletedProjects / TotalRelatedProjects;

        // Task Analysis
        public int TotalRelatedTasks => RelatedTasks.Count;
        public int CompletedTasks => RelatedTasks.Count(t => t.IsCompleted);
        public double TaskCompletionRate => 
            TotalRelatedTasks == 0 ? 0 : (double)CompletedTasks / TotalRelatedTasks;

        // Overall Progress Calculation
        public double CalculatedProgress
        {
            get
            {
                if (TotalRelatedProjects == 0 && TotalRelatedTasks == 0)
                {
                    return Goal.Progress;
                }

                const double projectWeight = 0.7;
                const double taskWeight = 0.3;

                if (TotalRelatedProjects == 0)
                {
                    return TaskCompletionRate * 100;
                }

                if (TotalRelatedTasks == 0)
                {
                    return ProjectCompletionRate * 100;
                }

                return (ProjectCompletionRate * projectWeight + 
                        TaskCompletionRate * taskWeight) * 100;
            }
        }

        public bool ShouldUpdateProgress => Math.Abs(CalculatedProgress - Goal.Progress) > 5;

        // Achievement Tracking
        public int TotalAchievements => Achievements.Count;
        public int TotalPoints => Achievements.Sum(a => 0); // Would need Achievement.Points reference

        public Dictionary<string, object> GetMetrics()
        {
            return new Dictionary<string, object>
            {
                ["goalId"] = Goal.Id,
                ["goalTitle"] = Goal.Title,
                ["goalProgress"] = Goal.Progress,
                ["totalRelatedProjects"] = TotalRelatedProjects,
                ["completedProjects"] = CompletedProjects,
                ["projectCompletionRate"] = ProjectCompletionRate,
                ["totalRelatedTasks"] = TotalRelatedTasks,
                ["completedTasks"] = CompletedTasks,
                ["taskCompletionRate"] = TaskCompletionRate,
                ["calculatedProgress"] = CalculatedProgress,
                ["shouldUpdateProgress"] = ShouldUpdateProgress,
                ["totalAchievements"] = TotalAchievements
            };
        }
    }

    public class ProjectSummary
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
        public int Progress { get; set; }
    }

    public class TaskSummary
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
    }
}
