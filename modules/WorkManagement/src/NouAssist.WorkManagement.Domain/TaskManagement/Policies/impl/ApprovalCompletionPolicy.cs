using System.Linq;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using NouAssist.WorkManagement.TaskManagement.Aggregates.TaskInstance;

namespace NouAssist.WorkManagement.TaskManagement.Policies.impl;

public class ApprovalCompletionPolicy : ITaskCompletionPolicy
{
    public PolicyResult CanComplete(TaskInstance taskInstance)
    {
        // Check if all required checklist items are completed
        if (taskInstance.ChecklistItems.Any(ci => !ci.IsChecked))
        {
            return PolicyResult.Deny("Tasks:RequiredChecklistItemsNotCompleted");
        }

        // Check if task requires approval and has approval request linked
        if (taskInstance.RequiresApproval && !taskInstance.ApprovalRequestId.HasValue)
        {
            return PolicyResult.Deny("Tasks:ApprovalRequired");
        }

        // Check if task is blocked
        if (taskInstance.IsBlocked)
        {
            return PolicyResult.Deny("Tasks:TaskIsBlocked");
        }

        // Check if task has blocking dependencies
        if (taskInstance.HasBlockingDependencies)
        {
            return PolicyResult.Deny("Tasks:HasBlockingDependencies");
        }

        return PolicyResult.Allow();
    }
}