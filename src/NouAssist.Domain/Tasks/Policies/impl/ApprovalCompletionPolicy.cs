using System.Linq;
using NouAssist.Tasks.Aggregates;

namespace NouAssist.Tasks.Policies.impl;

public class ApprovalCompletionPolicy : ITaskCompletionPolicy
{
    public (bool CanComplete, string FailureReason) Check(TaskInstance taskInstance)
    {
        if (taskInstance.StepCompletions.Any(sc => !sc.IsDone))
        {
            return (false, "Tasks:RequiredStepsNotCompleted");
        }

        if (taskInstance.RequiresApproval && !taskInstance.ApprovalRequestId.HasValue) // Or whatever your logic is
        {
            return (false, "Tasks:ApprovalRequired");
        }

        return (true, string.Empty);
    }
}