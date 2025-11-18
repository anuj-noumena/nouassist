using NouAssist.WorkManagement.TaskManagement.Aggregates;
using NouAssist.WorkManagement.TaskManagement.Aggregates.TaskInstance;

namespace NouAssist.WorkManagement.TaskManagement.Policies;

public interface ITaskCompletionPolicy
{
    PolicyResult CanComplete(TaskInstance taskInstance);
}

public class PolicyResult
{
    public bool IsAllowed { get; private set; }
    public string Reason { get; private set; }

    private PolicyResult(bool isAllowed, string reason = null)
    {
        IsAllowed = isAllowed;
        Reason = reason;
    }

    public static PolicyResult Allow() => new PolicyResult(true);
    
    public static PolicyResult Deny(string reason) => new PolicyResult(false, reason);
}
