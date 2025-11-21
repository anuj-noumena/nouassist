using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Aggregates;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Enums;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Policies
{
    /// <summary>
    /// Policy for validating prospect status transitions
    /// </summary>
    public interface IProspectTransitionPolicy
    {
        /// <summary>
        /// Checks if a prospect can transition to a new status
        /// </summary>
        PolicyResult CanTransition(Prospect prospect, ProspectStatus targetStatus);
    }

    /// <summary>
    /// Result of a policy evaluation
    /// </summary>
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
}
