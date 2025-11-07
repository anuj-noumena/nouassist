using System.Linq;
using NouAssist.Tasks.Aggregates;

namespace NouAssist.Tasks.Policies.impl;

public class DefaultTaskStartPolicy
{
    public (bool CanComplete, string FailureReason) Check(TaskInstance taskInstance)
    {
        if (taskInstance.Schedule.HasStarted)
        {
            return (false, "Tasks:AlreadyStarted");
        }

        if (taskInstance.BlockingInfo.IsBlocked)
        {
            return (false, "Tasks:CannotStartBlockedTask");
        }

        return (true, string.Empty);
    }
}