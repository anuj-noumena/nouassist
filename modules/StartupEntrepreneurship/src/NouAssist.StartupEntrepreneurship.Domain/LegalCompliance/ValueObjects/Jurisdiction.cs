using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.ValueObjects
{
    /// <summary>
    /// Value object representing a legal jurisdiction
    /// </summary>
    public class Jurisdiction : ValueObject
    {
        public string Country { get; private set; }
        public string State { get; private set; }
        public string Description { get; private set; }

        protected Jurisdiction() { }

        public Jurisdiction(string country, string state = null, string description = null)
        {
            Country = Check.NotNullOrWhiteSpace(
                country,
                nameof(country),
                maxLength: LegalComplianceConsts.MaxJurisdictionLength
            );
            State = state;
            Description = description;
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(State) ? Country : $"{Country}, {State}";
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Country;
            yield return State;
            yield return Description;
        }
    }
}
