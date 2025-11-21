using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.ValueObjects
{
    /// <summary>
    /// Value object representing an estimated valuation of an intellectual property asset
    /// </summary>
    public class IPValuationEstimate : ValueObject
    {
        public decimal EstimatedValue { get; private set; }
        public string Currency { get; private set; }
        public DateTime ValuationDate { get; private set; }
        public string ValuationMethod { get; private set; }
        public string ValuationNotes { get; private set; }

        protected IPValuationEstimate() { }

        public IPValuationEstimate(
            decimal estimatedValue,
            string currency,
            DateTime valuationDate,
            string valuationMethod,
            string valuationNotes = null)
        {
            if (estimatedValue < 0)
            {
                throw new BusinessException("LegalCompliance:Validation:InvalidValue", 
                    "Estimated value cannot be negative");
            }

            EstimatedValue = estimatedValue;
            Currency = Check.NotNullOrWhiteSpace(currency, nameof(currency), maxLength: 3);
            ValuationDate = valuationDate;
            ValuationMethod = Check.NotNullOrWhiteSpace(valuationMethod, nameof(valuationMethod));
            ValuationNotes = valuationNotes?.Length > LegalComplianceConsts.MaxNotesLength
                ? valuationNotes.Substring(0, LegalComplianceConsts.MaxNotesLength)
                : valuationNotes;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return EstimatedValue;
            yield return Currency;
            yield return ValuationDate;
            yield return ValuationMethod;
            yield return ValuationNotes;
        }
    }
}
