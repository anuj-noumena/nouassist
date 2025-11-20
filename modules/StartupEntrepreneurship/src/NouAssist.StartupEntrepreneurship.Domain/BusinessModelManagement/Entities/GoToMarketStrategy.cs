namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Entities
{
    /// <summary>
    /// Represents a go-to-market strategy within a revenue model
    /// </summary>
    public class GoToMarketStrategy : Entity<Guid>
    {
        public Guid RevenueModelId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string TargetMarket { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public bool IsActive { get; private set; }

        private readonly List<GtmPlanElement> _planElements;
        public IReadOnlyCollection<GtmPlanElement> PlanElements => _planElements.AsReadOnly();

        protected GoToMarketStrategy()
        {
            _planElements = new List<GtmPlanElement>();
        }

        internal GoToMarketStrategy(
            Guid id,
            Guid revenueModelId,
            string name,
            string description,
            string targetMarket,
            DateTime startDate,
            DateTime? endDate = null) : base(id)
        {
            RevenueModelId = revenueModelId;
            SetName(name);
            Description = description;
            TargetMarket = targetMarket;
            StartDate = startDate;
            EndDate = endDate;
            IsActive = true;
            _planElements = new List<GtmPlanElement>();
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: BusinessModelManagementConsts.GoToMarket.MaxStrategyNameLength
            );
        }

        public void AddPlanElement(GtmPlanElement element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            _planElements.Add(element);
        }

        public void RemovePlanElement(GtmPlanElement element)
        {
            if (element != null)
            {
                _planElements.Remove(element);
            }
        }

        public void UpdateDescription(string description)
        {
            Description = description;
        }

        public void UpdateTargetMarket(string targetMarket)
        {
            TargetMarket = targetMarket;
        }

        public void UpdateTimeline(DateTime startDate, DateTime? endDate = null)
        {
            if (endDate.HasValue && endDate.Value <= startDate)
            {
                throw new BusinessModelManagementDomainException("BusinessModelManagement:ErrorMessage:InvalidValue")
                    .WithData("propertyName", "EndDate");
            }

            StartDate = startDate;
            EndDate = endDate;
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
