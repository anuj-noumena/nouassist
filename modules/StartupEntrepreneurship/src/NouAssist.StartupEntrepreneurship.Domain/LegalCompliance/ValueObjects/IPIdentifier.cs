using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Constants;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.ValueObjects
{
    /// <summary>
    /// Value object representing an intellectual property identifier (e.g., patent number, trademark registration)
    /// </summary>
    public class IPIdentifier : ValueObject
    {
        public string Value { get; private set; }
        public IntellectualPropertyType Type { get; private set; }
        public string IssuingAuthority { get; private set; }

        protected IPIdentifier() { }

        public IPIdentifier(string value, IntellectualPropertyType type, string issuingAuthority)
        {
            SetValue(value);
            Type = type;
            SetIssuingAuthority(issuingAuthority);
        }

        private void SetValue(string value)
        {
            Value = Check.NotNullOrWhiteSpace(
                value,
                nameof(value),
                maxLength: LegalComplianceConsts.MaxIPIdentifierLength
            );
        }

        private void SetIssuingAuthority(string issuingAuthority)
        {
            IssuingAuthority = Check.NotNullOrWhiteSpace(
                issuingAuthority,
                nameof(issuingAuthority),
                maxLength: LegalComplianceConsts.MaxJurisdictionLength
            );
        }

        public override string ToString()
        {
            return $"{Value} ({IssuingAuthority})";
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
            yield return Type;
            yield return IssuingAuthority;
        }
    }
}
