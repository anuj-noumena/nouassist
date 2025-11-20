namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Entities
{
    /// <summary>
    /// Represents a pricing model within a revenue model
    /// </summary>
    public class PricingModel : Entity<Guid>
    {
        public Guid RevenueModelId { get; private set; }
        public string Name { get; private set; }
        public PricingStrategy Strategy { get; private set; }
        public string Description { get; private set; }
        public bool IsActive { get; private set; }

        private readonly List<PricingTier> _pricingTiers;
        public IReadOnlyCollection<PricingTier> PricingTiers => _pricingTiers.AsReadOnly();

        protected PricingModel()
        {
            _pricingTiers = new List<PricingTier>();
        }

        internal PricingModel(
            Guid id,
            Guid revenueModelId,
            string name,
            PricingStrategy strategy,
            string description = null) : base(id)
        {
            RevenueModelId = revenueModelId;
            SetName(name);
            Strategy = strategy;
            Description = description;
            IsActive = true;
            _pricingTiers = new List<PricingTier>();
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: BusinessModelManagementConsts.RevenueModel.MaxPricingModelNameLength
            );
        }

        public void AddPricingTier(PricingTier tier)
        {
            if (tier == null)
                throw new ArgumentNullException(nameof(tier));

            if (_pricingTiers.Any(t => t.Name == tier.Name))
            {
                throw new BusinessModelManagementDomainException("BusinessModelManagement:ErrorMessage:DuplicateName")
                    .WithData("entityType", "PricingTier")
                    .WithData("name", tier.Name);
            }

            _pricingTiers.Add(tier);
        }

        public void RemovePricingTier(string tierName)
        {
            var tier = _pricingTiers.FirstOrDefault(t => t.Name == tierName);
            if (tier != null)
            {
                _pricingTiers.Remove(tier);
            }
        }

        public void UpdateDescription(string description)
        {
            Description = description;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }
    }
}
