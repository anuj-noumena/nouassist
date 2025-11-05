using NouAssist.Tasks.Aggregates;

namespace NouAssist.Tasks.Policies;

public interface ITaskCompletionPolicy
{
    (bool CanComplete, string FailureReason) Check(TaskInstance taskInstance);
}