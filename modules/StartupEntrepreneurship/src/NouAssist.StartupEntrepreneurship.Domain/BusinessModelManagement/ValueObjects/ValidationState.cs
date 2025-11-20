namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.ValueObjects
{
    /// <summary>
    /// Represents the validation state of a canvas component or hypothesis
    /// </summary>
    public class ValidationState : ValueObject
    {
        public ValidationStatus Status { get; private set; }
        public double ConfidenceLevel { get; private set; }
        public DateTime LastUpdated { get; private set; }
        public string Notes { get; private set; }

        protected ValidationState() { }

        public ValidationState(ValidationStatus status, double confidenceLevel = 0.0, string notes = null)
        {
            Status = status;
            SetConfidenceLevel(confidenceLevel);
            LastUpdated = DateTime.UtcNow;
            Notes = notes;
        }

        private void SetConfidenceLevel(double confidenceLevel)
        {
            if (confidenceLevel < 0.0 || confidenceLevel > 1.0)
            {
                throw new BusinessModelManagementDomainException("BusinessModelManagement:ErrorMessage:InvalidValue")
                    .WithData("propertyName", "confidenceLevel")
                    .WithData("value", confidenceLevel);
            }

            ConfidenceLevel = confidenceLevel;
        }

        public ValidationState UpdateStatus(ValidationStatus newStatus, double newConfidenceLevel, string notes = null)
        {
            return new ValidationState(newStatus, newConfidenceLevel, notes);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Status;
            yield return ConfidenceLevel;
            yield return LastUpdated.Date; // Ignore time component for equality
            yield return Notes;
        }
    }
}
