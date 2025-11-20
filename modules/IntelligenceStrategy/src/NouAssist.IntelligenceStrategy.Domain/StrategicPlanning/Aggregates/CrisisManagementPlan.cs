namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Aggregates
{
    /// <summary>
    /// Crisis Management Plan aggregate root.
    /// Manages contingency plans, protocols, and crisis response procedures.
    /// </summary>
    public class CrisisManagementPlan : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public CrisisLevel MaxCrisisLevel { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime? ActivatedAt { get; private set; }
        public Guid? ActivatedBy { get; private set; }
        public DateTime? LastReviewDate { get; private set; }
        public DateTime NextReviewDate { get; private set; }

        private readonly List<ContingencyPlanItem> _contingencyItems;
        public IReadOnlyCollection<ContingencyPlanItem> ContingencyItems => _contingencyItems.AsReadOnly();

        protected CrisisManagementPlan()
        {
            _contingencyItems = new List<ContingencyPlanItem>();
        }

        /// <summary>
        /// Factory method to create a new crisis management plan
        /// </summary>
        public static CrisisManagementPlan Create(
            Guid id,
            string name,
            string description,
            CrisisLevel maxCrisisLevel,
            DateTime nextReviewDate,
            Guid? tenantId = null)
        {
            return new CrisisManagementPlan(id, name, description, maxCrisisLevel, nextReviewDate, tenantId);
        }

        private CrisisManagementPlan(
            Guid id,
            string name,
            string description,
            CrisisLevel maxCrisisLevel,
            DateTime nextReviewDate,
            Guid? tenantId) : base(id)
        {
            SetName(name);
            SetDescription(description);
            MaxCrisisLevel = maxCrisisLevel;
            NextReviewDate = nextReviewDate;
            TenantId = tenantId;
            IsActive = false;
            _contingencyItems = new List<ContingencyPlanItem>();
        }

        /// <summary>
        /// Adds a contingency plan item
        /// </summary>
        public void AddContingencyItem(
            string title,
            string description,
            RiskScenario riskScenario,
            int priority)
        {
            var item = new ContingencyPlanItem(
                Guid.NewGuid(),
                Id,
                title,
                description,
                riskScenario,
                priority);

            _contingencyItems.Add(item);
        }

        /// <summary>
        /// Activates the crisis management plan
        /// </summary>
        public void Activate(Guid activatedBy, DateTime activatedAt, string reason = null)
        {
            if (IsActive)
            {
                throw new BusinessException("StrategicPlanning:ErrorMessage:CrisisAlreadyActive");
            }

            IsActive = true;
            ActivatedAt = activatedAt;
            ActivatedBy = activatedBy;

            AddDistributedEvent(new CrisisManagementPlanActivatedEto
            {
                Id = Id,
                PlanName = Name,
                Level = MaxCrisisLevel,
                Description = reason ?? Description,
                ActivatedAt = activatedAt,
                ActivatedBy = activatedBy,
                TenantId = TenantId,
                CreationTime = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Deactivates the crisis management plan
        /// </summary>
        public void Deactivate()
        {
            if (!IsActive)
            {
                throw new BusinessException("Crisis management plan is not currently active");
            }

            IsActive = false;
            ActivatedAt = null;
            ActivatedBy = null;
        }

        /// <summary>
        /// Updates the review schedule
        /// </summary>
        public void UpdateReviewSchedule(DateTime lastReviewDate, DateTime nextReviewDate)
        {
            LastReviewDate = lastReviewDate;
            NextReviewDate = nextReviewDate;
        }

        /// <summary>
        /// Checks if the plan is due for review
        /// </summary>
        public bool IsDueForReview => DateTime.UtcNow >= NextReviewDate;

        /// <summary>
        /// Gets contingency items for a specific crisis level
        /// </summary>
        public IEnumerable<ContingencyPlanItem> GetContingencyItemsForLevel(CrisisLevel level)
        {
            return _contingencyItems
                .Where(item => item.RiskScenario.Level <= level)
                .OrderBy(item => item.Priority);
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: StrategicPlanningConsts.MaxPlanNameLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: StrategicPlanningConsts.MaxContingencyDescriptionLength
            );
        }
    }
}
