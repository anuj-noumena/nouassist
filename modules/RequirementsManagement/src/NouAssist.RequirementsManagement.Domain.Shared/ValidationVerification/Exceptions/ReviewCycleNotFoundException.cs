using System;

namespace NouAssist.RequirementsManagement.ValidationVerification.Exceptions
{
    /// <summary>
    /// Exception thrown when a review cycle is not found
    /// </summary>
    public class ReviewCycleNotFoundException : ValidationVerificationDomainException
    {
        public ReviewCycleNotFoundException(Guid reviewCycleId)
            : base("ValidationVerification:ErrorMessage:ReviewCycleNotFound")
        {
            WithData("reviewCycleId", reviewCycleId);
        }
    }
}
