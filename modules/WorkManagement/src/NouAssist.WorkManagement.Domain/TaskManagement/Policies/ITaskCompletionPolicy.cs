using NouAssist.WorkManagement.TaskManagement.Aggregates;

namespace NouAssist.WorkManagement.TaskManagement.Policies;

public interface ITaskCompletionPolicy
{
    (bool CanComplete, string FailureReason) Check(TaskInstance taskInstance);
}