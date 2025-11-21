using System;

namespace NouAssist.RequirementsManagement.ValidationVerification.Exceptions
{
    /// <summary>
    /// Exception thrown when attempting to modify a review cycle that has already started
    /// </summary>
    public class ReviewCycleAlreadyStartedException : ValidationVerificationDomainException
    {
        public ReviewCycleAlreadyStartedException(Guid reviewCycleId)
            : base("ValidationVerification:ErrorMessage:ReviewCycleAlreadyStarted")
        {
            WithData("reviewCycleId", reviewCycleId);
        }
    }
}
