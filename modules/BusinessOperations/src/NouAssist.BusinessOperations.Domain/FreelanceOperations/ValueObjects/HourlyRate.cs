using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.BusinessOperations.FreelanceOperations.ValueObjects
{
    /// <summary>
    /// Value object representing an hourly rate for billing
    /// </summary>
    public class HourlyRate : ValueObject
    {
        /// <summary>
        /// The rate amount
        /// </summary>
        public decimal Amount { get; private set; }
        
        /// <summary>
        /// Currency code (e.g., USD, EUR)
        /// </summary>
        public string Currency { get; private set; }

        protected HourlyRate()
        {
        }

        public HourlyRate(decimal amount, string currency)
        {
            if (amount < 0)
            {
                throw new BusinessException("FreelanceOperations:Validation:InvalidValue", "Hourly rate");
            }
            
            if (string.IsNullOrWhiteSpace(currency))
            {
                throw new BusinessException("FreelanceOperations:Validation:Required", "Currency");
            }
            
            Amount = amount;
            Currency = currency.ToUpperInvariant();
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Amount;
            yield return Currency;
        }
        
        public string Format()
        {
            return $"{Amount} {Currency}/hr";
        }
        
        public override string ToString()
        {
            return Format();
        }
    }
}
