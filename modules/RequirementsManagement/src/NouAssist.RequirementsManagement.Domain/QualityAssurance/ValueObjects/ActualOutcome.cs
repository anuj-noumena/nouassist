namespace NouAssist.RequirementsManagement.QualityAssurance.ValueObjects
{
    /// <summary>
    /// Represents the actual outcome of a test run
    /// </summary>
    public class ActualOutcome : ValueObject
    {
        /// <summary>
        /// Description of what actually happened during the test
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// Optional log data or output from the test
        /// </summary>
        public string LogData { get; private set; }

        protected ActualOutcome() { }

        public ActualOutcome(string description, string logData = null)
        {
            SetDescription(description);
            LogData = logData;
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: QADomainConstants.MaxActualOutcomeLength
            );
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Description;
            yield return LogData ?? string.Empty;
        }
    }
}
