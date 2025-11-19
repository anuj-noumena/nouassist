using System.Linq;
using NouAssist.WorkManagement.TaskManagement.Aggregates;

namespace NouAssist.WorkManagement.TaskManagement.Policies.impl;

public class DefaultCompletionPolicy : ITaskCompletionPolicy
{
    public PolicyResult CanComplete(TaskInstance taskInstance)
    {
        // Check if task is already completed
        if (taskInstance.Schedule.IsCompleted)
        {
            return PolicyResult.Deny("Tasks:AlreadyCompleted");
        }

        // Check if task is blocked
        if (taskInstance.IsBlocked)
        {
            return PolicyResult.Deny("Tasks:TaskIsBlocked");
        }

        // Check if all checklist items are completed
        if (taskInstance.ChecklistItems.Any(ci => !ci.IsChecked))
        {
            return PolicyResult.Deny("Tasks:RequiredChecklistItemsNotCompleted");
        }

        // Check if task has blocking dependencies
        if (taskInstance.HasBlockingDependencies)
        {
            return PolicyResult.Deny("Tasks:HasBlockingDependencies");
        }

        return PolicyResult.Allow();
    }
}