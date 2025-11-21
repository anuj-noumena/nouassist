using System;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.Exceptions
{
    /// <summary>
    /// Exception thrown when a compliance deadline has expired
    /// </summary>
    public class ComplianceDeadlineExpiredException : LegalComplianceDomainException
    {
        public Guid ObligationId { get; }
        public DateTime Deadline { get; }

        public ComplianceDeadlineExpiredException(Guid obligationId, DateTime deadline)
            : base("LegalCompliance:ErrorMessage:ComplianceDeadlineExpired")
        {
            ObligationId = obligationId;
            Deadline = deadline;
            WithData("obligationId", obligationId);
            WithData("deadline", deadline);
        }
    }
}
