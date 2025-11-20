namespace NouAssist.StartupEntrepreneurship.LegalCompliance.ValueObjects
{
    /// <summary>
    /// Value object representing a description of a compliance obligation
    /// </summary>
    public class ObligationDescription : ValueObject
    {
        public string Overview { get; private set; }
        public string Requirements { get; private set; }
        public string Notes { get; private set; }

        protected ObligationDescription() { }

        public ObligationDescription(string overview, string requirements, string notes = null)
        {
            Overview = Check.NotNullOrWhiteSpace(
                overview,
                nameof(overview),
                maxLength: LegalComplianceConsts.MaxObligationDescriptionLength
            );
            
            Requirements = Check.NotNullOrWhiteSpace(
                requirements,
                nameof(requirements),
                maxLength: LegalComplianceConsts.MaxObligationDescriptionLength
            );
            
            Notes = notes?.Length > LegalComplianceConsts.MaxNotesLength
                ? notes.Substring(0, LegalComplianceConsts.MaxNotesLength)
                : notes;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Overview;
            yield return Requirements;
            yield return Notes;
        }
    }
}
