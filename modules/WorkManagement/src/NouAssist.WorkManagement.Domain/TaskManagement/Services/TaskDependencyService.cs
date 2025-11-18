using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using NouAssist.WorkManagement.TaskManagement.Aggregates.TaskInstance;
using NouAssist.WorkManagement.TaskManagement.Entities;
using NouAssist.WorkManagement.TaskManagement.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace NouAssist.WorkManagement.TaskManagement.Services
{
    public class TaskDependencyService : DomainService
    {
        public bool CanStartTask(
            TaskInstance taskInstance,
            List<TaskInstance> dependentInstances,
            List<TaskDependency> dependencies)
        {
            // Get blocking dependencies for this task instance
            var blockingDeps = dependencies
                .Where(d => d.TaskInstanceId == taskInstance.Id && d.IsBlocking)
                .ToList();

            foreach (var dependency in blockingDeps)
            {
                var dependentInstance = dependentInstances
                    .FirstOrDefault(i => i.Id == dependency.DependsOnTaskInstanceId);

                if (dependentInstance == null)
                {
                    throw new BusinessException("Tasks:DependentTaskNotFound");
                }

                // Check based on dependency type
                switch (dependency.DependencyType)
                {
                    case DependencyType.FinishToStart:
                        if (!dependentInstance.Schedule.IsCompleted)
                        {
                            return false;
                        }

                        break;

                    case DependencyType.StartToStart:
                        if (!dependentInstance.Schedule.HasStarted)
                        {
                            return false;
                        }

                        break;

                    case DependencyType.FinishToFinish:
                        // This doesn't block start
                        break;

                    case DependencyType.StartToFinish:
                        // Rare: successor can't finish until predecessor starts
                        break;
                }
            }

            return true;
        }

        public DateTime? CalculateEarliestStartDate(
            TaskInstance taskInstance,
            List<TaskInstance> dependentInstances,
            List<TaskDependency> dependencies)
        {
            var blockingDeps = dependencies
                .Where(d => d.TaskInstanceId == taskInstance.Id && d.IsBlocking)
                .ToList();

            if (!blockingDeps.Any())
            {
                return null; // Can start anytime
            }

            DateTime? earliestStart = null;

            foreach (var dependency in blockingDeps)
            {
                var dependentInstance = dependentInstances
                    .FirstOrDefault(i => i.Id == dependency.DependsOnTaskInstanceId);

                if (dependentInstance == null)
                {
                    continue;
                }

                DateTime? constraintDate = null;

                switch (dependency.DependencyType)
                {
                    case DependencyType.FinishToStart:
                        constraintDate = dependentInstance.Schedule.ActualDueDate ?? 
                                         dependentInstance.Schedule.PlannedDueDate;
                        break;

                    case DependencyType.StartToStart:
                        constraintDate = dependentInstance.Schedule.ActualStartDate ?? 
                                         dependentInstance.Schedule.PlannedStartDate;
                        break;
                }

                if (constraintDate.HasValue)
                {
                    // Apply lag
                    var adjustedDate = constraintDate.Value.Add(dependency.LagDuration);

                    if (!earliestStart.HasValue || adjustedDate > earliestStart.Value)
                    {
                        earliestStart = adjustedDate;
                    }
                }
            }

            return earliestStart;
        }

        public List<Guid> DetectCircularDependencies(
            Guid taskId,
            List<TaskDependency> allDependencies)
        {
            var visited = new HashSet<Guid>();
            var recursionStack = new HashSet<Guid>();
            var circularDeps = new List<Guid>();

            DetectCircularDependenciesRecursive(
                taskId, allDependencies, visited, recursionStack, circularDeps);

            return circularDeps;
        }

        private bool DetectCircularDependenciesRecursive(
            Guid currentTaskId,
            List<TaskDependency> allDependencies,
            HashSet<Guid> visited,
            HashSet<Guid> recursionStack,
            List<Guid> circularDeps)
        {
            visited.Add(currentTaskId);
            recursionStack.Add(currentTaskId);

            var dependencies = allDependencies
                .Where(d => d.TaskId == currentTaskId)
                .ToList();

            foreach (var dep in dependencies)
            {
                if (!dep.DependsOnTaskId.HasValue)
                {
                    continue;
                }

                var dependsOnId = dep.DependsOnTaskId.Value;

                if (!visited.Contains(dependsOnId))
                {
                    if (DetectCircularDependenciesRecursive(
                        dependsOnId, allDependencies, visited, recursionStack, circularDeps))
                    {
                        return true;
                    }
                }
                else if (recursionStack.Contains(dependsOnId))
                {
                    // Circular dependency detected
                    circularDeps.Add(dependsOnId);
                    return true;
                }
            }

            recursionStack.Remove(currentTaskId);
            return false;
        }
    }
}
