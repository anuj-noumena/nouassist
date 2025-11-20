using Volo.Abp.Domain.Values;

namespace NouAssist.RequirementsManagement.ValidationVerification.ValueObjects
{
    /// <summary>
    /// Represents the expected result of a test step
    /// </summary>
    public class ExpectedResult : ValueObject
    {
        public string Description { get; private set; }
        public string AcceptanceCriteria { get; private set; }

        private ExpectedResult() { }

        public ExpectedResult(string description, string acceptanceCriteria = null)
        {
            SetDescription(description);
            AcceptanceCriteria = acceptanceCriteria;
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: ValidationVerificationConsts.TestStep.MaxExpectedResultLength
            );
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Description;
            yield return AcceptanceCriteria;
        }
    }
}
