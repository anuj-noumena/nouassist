namespace NouAssist.StartupEntrepreneurship.LegalCompliance.ValueObjects
{
    /// <summary>
    /// Value object representing a key date for legal documents and compliance obligations
    /// </summary>
    public class KeyDate : ValueObject
    {
        public DateTime Date { get; private set; }
        public string Description { get; private set; }
        public bool IsRecurring { get; private set; }
        public string RecurrencePattern { get; private set; }

        protected KeyDate() { }

        public KeyDate(DateTime date, string description, bool isRecurring = false, string recurrencePattern = null)
        {
            Date = date;
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: LegalComplianceConsts.MaxAgreementDescriptionLength
            );
            IsRecurring = isRecurring;
            RecurrencePattern = recurrencePattern;
        }

        public DateTime CalculateNextDate()
        {
            if (!IsRecurring || string.IsNullOrEmpty(RecurrencePattern))
            {
                return Date;
            }

            // This is a simplified version - in a real system, we would parse the recurrence pattern
            // and calculate the next date based on that pattern
            return Date.AddYears(1); // Default annual recurrence
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Date;
            yield return Description;
            yield return IsRecurring;
            yield return RecurrencePattern;
        }
    }
}
