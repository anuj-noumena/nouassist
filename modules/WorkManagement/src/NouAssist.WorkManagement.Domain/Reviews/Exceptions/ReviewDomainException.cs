using System;
using Volo.Abp;

namespace NouAssist.WorkManagement.Reviews.Exceptions
{
    public class ReviewDomainException : BusinessException
    {
        public ReviewDomainException(string code, string? message = null, string? details = null, Exception? innerException = null)
            : base(code, message, details, innerException)
        {
        }

        public static ReviewDomainException ReviewAlreadyCompleted()
        {
            return new ReviewDomainException(
                "Reviews:AlreadyCompleted",
                "Review has already been completed.");
        }

        public static ReviewDomainException InvalidReviewType()
        {
            return new ReviewDomainException(
                "Reviews:InvalidType",
                "Invalid review type specified.");
        }

        public static ReviewDomainException ReviewNotDue()
        {
            return new ReviewDomainException(
                "Reviews:NotDue",
                "Review is not yet due for completion.");
        }

        public static ReviewDomainException MissingRequiredSections()
        {
            return new ReviewDomainException(
                "Reviews:MissingRequiredSections",
                "Review cannot be completed with missing required sections.");
        }
    }
}
