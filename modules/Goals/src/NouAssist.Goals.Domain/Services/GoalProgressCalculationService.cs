using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.Goals.Aggregates;
using Volo.Abp.Domain.Services;

namespace NouAssist.Goals.Services
{
    public class GoalProgressCalculationService : DomainService
    {
        public int CalculateProgressFromMilestones(Goal goal)
        {
            var milestones = goal.Milestones.ToList();
            if (milestones.Count == 0)
            {
                return goal.Progress;
            }

            var completedCount = milestones.Count(m => m.IsCompleted);
            return (int)Math.Round((double)completedCount / milestones.Count * 100);
        }

        public int CalculateProgressFromRelatedEntities(
            int completedProjects, 
            int totalProjects,
            int completedTasks,
            int totalTasks)
        {
            if (totalProjects == 0 && totalTasks == 0)
                return 0;

            const double projectWeight = 0.7;
            const double taskWeight = 0.3;

            double projectProgress = totalProjects > 0 
                ? (double)completedProjects / totalProjects 
                : 0;

            double taskProgress = totalTasks > 0 
                ? (double)completedTasks / totalTasks 
                : 0;

            if (totalProjects == 0)
                return (int)Math.Round(taskProgress * 100);

            if (totalTasks == 0)
                return (int)Math.Round(projectProgress * 100);

            var weightedProgress = (projectProgress * projectWeight) + 
                                   (taskProgress * taskWeight);

            return (int)Math.Round(weightedProgress * 100);
        }

        public bool ShouldAutoUpdateProgress(Goal goal, int calculatedProgress)
        {
            return Math.Abs(calculatedProgress - goal.Progress) > 5;
        }
    }
}
