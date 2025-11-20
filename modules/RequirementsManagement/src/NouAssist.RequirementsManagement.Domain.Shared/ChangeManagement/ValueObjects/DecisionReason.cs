using Volo.Abp.Domain.Values;

namespace NouAssist.RequirementsManagement.ChangeManagement.ValueObjects
{
    /// <summary>
    /// Represents the reason for an approval decision
    /// </summary>
    public class DecisionReason : ValueObject
    {
        public string Value { get; private set; }

        private DecisionReason() { }

        public DecisionReason(string value)
        {
            SetValue(value);
        }

        private void SetValue(string value)
        {
            Value = Check.NotNullOrWhiteSpace(
                value,
                nameof(value),
                maxLength: ChangeManagementConsts.MaxDecisionReasonLength
            );
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        public override string ToString()
        {
            return Value;
        }

        public static implicit operator string(DecisionReason reason)
        {
            return reason?.Value;
        }
    }
}
