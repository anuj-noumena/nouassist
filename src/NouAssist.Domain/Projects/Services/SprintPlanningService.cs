using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.Projects.Aggregates;
using NouAssist.Projects.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace NouAssist.Projects.Services
{
    public class SprintPlanningService : DomainService
    {
        public SprintPlanResult PlanSprint(
            Project project,
            DateTime startDate,
            DateTime endDate,
            int availableCapacity,
            List<TaskEstimate> backlogItems)
        {
            var result = new SprintPlanResult
            {
                StartDate = startDate,
                EndDate = endDate,
                AvailableCapacity = availableCapacity
            };

            // Validate sprint duration
            var duration = (endDate - startDate).Days;
            if (duration < ProjectDomainConstants.MinSprintDurationDays || 
                duration > ProjectDomainConstants.MaxSprintDurationDays)
            {
                result.AddError($"Sprint duration must be between {ProjectDomainConstants.MinSprintDurationDays} and {ProjectDomainConstants.MaxSprintDurationDays} days.");
                return result;
            }

            // Sort backlog by priority
            var sortedBacklog = backlogItems
                .OrderByDescending(t => t.Priority)
                .ThenBy(t => t.StoryPoints)
                .ToList();

            // Fill sprint up to capacity
            int usedCapacity = 0;
            foreach (var item in sortedBacklog)
            {
                if (usedCapacity + item.StoryPoints <= availableCapacity)
                {
                    result.PlannedItems.Add(item);
                    usedCapacity += item.StoryPoints;
                }
            }

            result.PlannedStoryPoints = usedCapacity;
            result.IsSuccessful = true;

            return result;
        }

        public int CalculateTeamVelocity(List<ProjectSprint> completedSprints, int lastNSprints = 3)
        {
            if (!completedSprints.Any())
                return ProjectDomainConstants.DefaultSprintVelocity;

            var recentSprints = completedSprints
                .OrderByDescending(s => s.CompletedAt)
                .Take(lastNSprints)
                .ToList();

            return (int)recentSprints.Average(s => s.ActualVelocity);
        }
    }

    public class SprintPlanResult
    {
        public bool IsSuccessful { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int AvailableCapacity { get; set; }
        public int PlannedStoryPoints { get; set; }
        public List<TaskEstimate> PlannedItems { get; set; } = new();
        public List<string> Errors { get; set; } = new();

        public void AddError(string error) => Errors.Add(error);
    }

    public class TaskEstimate
    {
        public Guid TaskId { get; set; }
        public string Title { get; set; }
        public int StoryPoints { get; set; }
        public double Priority { get; set; }
    }
}
