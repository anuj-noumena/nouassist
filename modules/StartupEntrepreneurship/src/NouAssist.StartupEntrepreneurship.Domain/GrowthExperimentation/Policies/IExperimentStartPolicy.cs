using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Aggregates;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Policies
{
    /// <summary>
    /// Policy for validating if an experiment can start
    /// </summary>
    public interface IExperimentStartPolicy
    {
        /// <summary>
        /// Checks if an experiment can be started
        /// </summary>
        PolicyResult CanStart(GrowthExperiment experiment);
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
