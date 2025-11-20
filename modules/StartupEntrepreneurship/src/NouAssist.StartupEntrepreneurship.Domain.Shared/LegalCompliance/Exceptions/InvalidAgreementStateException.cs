namespace NouAssist.StartupEntrepreneurship.LegalCompliance.Exceptions
{
    /// <summary>
    /// Exception thrown when attempting to perform an operation on an agreement in an invalid state
    /// </summary>
    public class InvalidAgreementStateException : LegalComplianceDomainException
    {
        public Guid AgreementId { get; }
        public FilingStatusType CurrentStatus { get; }
        public FilingStatusType RequiredStatus { get; }

        public InvalidAgreementStateException(Guid agreementId, FilingStatusType currentStatus, FilingStatusType requiredStatus)
            : base("LegalCompliance:ErrorMessage:InvalidAgreementState")
        {
            AgreementId = agreementId;
            CurrentStatus = currentStatus;
            RequiredStatus = requiredStatus;
            WithData("agreementId", agreementId);
            WithData("currentStatus", currentStatus);
            WithData("requiredStatus", requiredStatus);
        }
    }
}
