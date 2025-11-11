using System.Linq;
using NouAssist.WorkManagement.TaskManagement.Aggregates;

namespace NouAssist.WorkManagement.TaskManagement.Policies.impl;

public class DefaultCompletionPolicy : ITaskCompletionPolicy
{
    public (bool CanComplete, string FailureReason) Check(TaskInstance taskInstance)
    {
        if (taskInstance.Schedule.IsCompleted)
        {
            return (false, "Tasks:AlreadyCompleted");
        }

        if (taskInstance.StepCompletions.Any(sc => !sc.IsDone))
        {
            return (false, "Tasks:RequiredStepsNotCompleted");
        }
        return (true, string.Empty);
    }
}