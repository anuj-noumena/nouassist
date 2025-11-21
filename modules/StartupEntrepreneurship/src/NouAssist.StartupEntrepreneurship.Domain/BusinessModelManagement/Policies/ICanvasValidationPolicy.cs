using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Aggregates;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Policies
{
    /// <summary>
    /// Policy for validating business model canvas completeness
    /// </summary>
    public interface ICanvasValidationPolicy
    {
        /// <summary>
        /// Checks if a canvas can be validated
        /// </summary>
        PolicyResult CanValidate(BusinessModelCanvas canvas);
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
