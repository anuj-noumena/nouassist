namespace NouAssist.BusinessOperations.FreelanceOperations.ValueObjects
{
    /// <summary>
    /// Value object representing a pricing structure for a proposal
    /// </summary>
    public class PricingStructure : ValueObject
    {
        /// <summary>
        /// Type of billing for this pricing
        /// </summary>
        public BillingType BillingType { get; private set; }
        
        /// <summary>
        /// Hourly rate if applicable
        /// </summary>
        public HourlyRate HourlyRate { get; private set; }
        
        /// <summary>
        /// Fixed price amount if applicable
        /// </summary>
        public decimal? FixedPrice { get; private set; }
        
        /// <summary>
        /// Currency for fixed price
        /// </summary>
        public string Currency { get; private set; }
        
        /// <summary>
        /// Estimated hours if applicable
        /// </summary>
        public int? EstimatedHours { get; private set; }
        
        /// <summary>
        /// Payment terms description
        /// </summary>
        public string PaymentTerms { get; private set; }

        protected PricingStructure()
        {
        }

        public static PricingStructure CreateHourlyRate(
            HourlyRate hourlyRate, 
            int? estimatedHours = null,
            string paymentTerms = null)
        {
            return new PricingStructure
            {
                BillingType = BillingType.Hourly,
                HourlyRate = hourlyRate ?? throw new BusinessException("FreelanceOperations:Validation:Required", "Hourly rate"),
                EstimatedHours = estimatedHours,
                PaymentTerms = paymentTerms
            };
        }

        public static PricingStructure CreateFixedPrice(
            decimal fixedPrice, 
            string currency,
            string paymentTerms = null)
        {
            if (fixedPrice < 0)
            {
                throw new BusinessException("FreelanceOperations:Validation:InvalidValue", "Fixed price");
            }
            
            if (string.IsNullOrWhiteSpace(currency))
            {
                throw new BusinessException("FreelanceOperations:Validation:Required", "Currency");
            }
            
            return new PricingStructure
            {
                BillingType = BillingType.FixedPrice,
                FixedPrice = fixedPrice,
                Currency = currency.ToUpperInvariant(),
                PaymentTerms = paymentTerms
            };
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return BillingType;
            yield return HourlyRate;
            yield return FixedPrice;
            yield return Currency ?? string.Empty;
            yield return EstimatedHours;
            yield return PaymentTerms ?? string.Empty;
        }
        
        public string GetFormattedPrice()
        {
            return BillingType switch
            {
                BillingType.Hourly => HourlyRate.ToString() + (EstimatedHours.HasValue ? $" (Est. {EstimatedHours} hours)" : ""),
                BillingType.FixedPrice => $"{FixedPrice} {Currency}",
                _ => "Custom pricing"
            };
        }
    }
}
