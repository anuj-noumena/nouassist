namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.ValueObjects
{
    /// <summary>
    /// Represents a pricing tier in a revenue model
    /// </summary>
    public class PricingTier : ValueObject
    {
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public string Currency { get; private set; }
        public List<string> Features { get; private set; }
        public int? MaxUsers { get; private set; }
        public string BillingPeriod { get; private set; } // Monthly, Yearly, etc.

        protected PricingTier()
        {
            Features = new List<string>();
        }

        public PricingTier(string name, decimal price, string currency, string billingPeriod = "Monthly", 
                          List<string> features = null, int? maxUsers = null)
        {
            SetName(name);
            SetPrice(price);
            SetCurrency(currency);
            BillingPeriod = billingPeriod ?? "Monthly";
            Features = features ?? new List<string>();
            MaxUsers = maxUsers;
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: 100
            );
        }

        private void SetPrice(decimal price)
        {
            if (price < 0)
            {
                throw new BusinessModelManagementDomainException("BusinessModelManagement:Validation:PositiveNumber")
                    .WithData("propertyName", "Price");
            }

            Price = price;
        }

        private void SetCurrency(string currency)
        {
            Currency = Check.NotNullOrWhiteSpace(currency, nameof(currency), maxLength: 3);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
            yield return Price;
            yield return Currency;
            yield return BillingPeriod;
            yield return MaxUsers;
            foreach (var feature in Features.OrderBy(f => f))
            {
                yield return feature;
            }
        }
    }
}
