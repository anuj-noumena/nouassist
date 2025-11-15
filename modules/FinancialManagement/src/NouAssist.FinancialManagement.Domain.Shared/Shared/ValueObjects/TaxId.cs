using System.Collections.Generic;
using System.Text.RegularExpressions;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.FinancialManagement.Shared.ValueObjects
{
    public class TaxId : ValueObject
    {
        public string Value { get; private set; }
        public string Country { get; private set; }

        protected TaxId() { }

        public TaxId(string value, string country = "US")
        {
            Value = Check.NotNullOrWhiteSpace(value, nameof(value));
            Country = Check.NotNullOrWhiteSpace(country, nameof(country)).ToUpperInvariant();
            
            ValidateFormat(value, country);
        }

        private static void ValidateFormat(string value, string country)
        {
            switch (country)
            {
                case "US": // EIN format: XX-XXXXXXX
                    if (!Regex.IsMatch(value, @"^\d{2}-?\d{7}$"))
                        throw new BusinessException("Financial:InvalidTaxId", 
                            "US Tax ID must be in format XX-XXXXXXX");
                    break;
                // Add other country formats as needed
            }
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
            yield return Country;
        }
    }
}
