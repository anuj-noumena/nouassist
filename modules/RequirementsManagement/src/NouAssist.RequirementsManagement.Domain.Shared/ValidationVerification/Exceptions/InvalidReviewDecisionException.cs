using System;

namespace NouAssist.RequirementsManagement.ValidationVerification.Exceptions
{
    /// <summary>
    /// Exception thrown when an invalid review decision is made
    /// </summary>
    public class InvalidReviewDecisionException : ValidationVerificationDomainException
    {
        public InvalidReviewDecisionException(Guid reviewerId, string reason)
            : base("ValidationVerification:ErrorMessage:InvalidReviewDecision")
        {
            WithData("reviewerId", reviewerId);
            WithData("reason", reason);
        }
    }
}
