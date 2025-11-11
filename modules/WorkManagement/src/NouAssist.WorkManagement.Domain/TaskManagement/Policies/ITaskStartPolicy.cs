using NouAssist.Tasks.Aggregates;

namespace NouAssist.Tasks.Policies;

public interface ITaskStartPolicy
{
    (bool CanStart, string FailureReason) Check(TaskInstance taskInstance);
}