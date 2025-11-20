using Volo.Abp.Domain.Values;

namespace NouAssist.RequirementsManagement.ChangeManagement.ValueObjects
{
    /// <summary>
    /// Represents a unique identifier for a change request
    /// </summary>
    public class RequestIdentifier : ValueObject
    {
        public string Value { get; private set; }

        private RequestIdentifier() { }

        public RequestIdentifier(string value)
        {
            SetValue(value);
        }

        private void SetValue(string value)
        {
            Value = Check.NotNullOrWhiteSpace(
                value,
                nameof(value),
                maxLength: ChangeManagementConsts.MaxRequestIdentifierLength
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

        public static implicit operator string(RequestIdentifier requestIdentifier)
        {
            return requestIdentifier?.Value;
        }
    }
}
