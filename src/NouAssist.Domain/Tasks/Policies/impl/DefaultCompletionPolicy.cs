using System.Linq;
using NouAssist.Tasks.Aggregates;

namespace NouAssist.Tasks.Policies.impl;

public class DefaultCompletionPolicy : ITaskCompletionPolicy
{
    public (bool CanComplete, string FailureReason) Check(TaskInstance taskInstance)
    {
        if (taskInstance.StepCompletions.Any(sc => !sc.IsDone))
        {
            return (false, "Tasks:RequiredStepsNotCompleted");
        }
        return (true, string.Empty);
    }
}