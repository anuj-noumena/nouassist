namespace NouAssist.StartupEntrepreneurship.LegalCompliance.ValueObjects
{
    /// <summary>
    /// Value object representing a status value with date and notes
    /// </summary>
    public class StatusValue : ValueObject
    {
        public ComplianceStatus Status { get; private set; }
        public DateTime StatusDate { get; private set; }
        public string Notes { get; private set; }

        protected StatusValue() { }

        public StatusValue(ComplianceStatus status, DateTime statusDate, string notes = null)
        {
            Status = status;
            StatusDate = statusDate;
            Notes = notes?.Length > LegalComplianceConsts.MaxNotesLength
                ? notes.Substring(0, LegalComplianceConsts.MaxNotesLength)
                : notes;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Status;
            yield return StatusDate;
            yield return Notes;
        }
    }
}
