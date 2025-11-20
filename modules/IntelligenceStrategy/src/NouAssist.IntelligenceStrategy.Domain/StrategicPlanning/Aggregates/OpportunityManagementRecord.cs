namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Aggregates
{
    /// <summary>
    /// Opportunity Management Record aggregate root.
    /// Manages opportunity filters, focus protection, cost calculations, and strategic experiments.
    /// </summary>
    public class OpportunityManagementRecord : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public string RecordName { get; private set; }
        public string Description { get; private set; }
        public FocusLevel CurrentFocusLevel { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public bool IsActive { get; private set; }

        private readonly List<OpportunityFilterRule> _filterRules;
        public IReadOnlyCollection<OpportunityFilterRule> FilterRules => _filterRules.AsReadOnly();

        private readonly List<string> _strategicExperiments;
        public IReadOnlyCollection<string> StrategicExperiments => _strategicExperiments.AsReadOnly();

        protected OpportunityManagementRecord()
        {
            _filterRules = new List<OpportunityFilterRule>();
            _strategicExperiments = new List<string>();
        }

        /// <summary>
        /// Factory method to create a new opportunity management record
        /// </summary>
        public static OpportunityManagementRecord Create(
            Guid id,
            string recordName,
            string description,
            FocusLevel initialFocusLevel,
            Guid? tenantId = null)
        {
            return new OpportunityManagementRecord(id, recordName, description, initialFocusLevel, tenantId);
        }

        private OpportunityManagementRecord(
            Guid id,
            string recordName,
            string description,
            FocusLevel initialFocusLevel,
            Guid? tenantId) : base(id)
        {
            SetRecordName(recordName);
            SetDescription(description);
            CurrentFocusLevel = initialFocusLevel;
            TenantId = tenantId;
            CreatedDate = DateTime.UtcNow;
            IsActive = true;
            _filterRules = new List<OpportunityFilterRule>();
            _strategicExperiments = new List<string>();
        }

        /// <summary>
        /// Adds a new opportunity filter rule
        /// </summary>
        public void AddFilterRule(
            string ruleName,
            string criterionName,
            string criterionDescription,
            decimal minThreshold,
            decimal maxThreshold,
            int weight,
            int priority,
            string createdBy)
        {
            var criterion = new FilterCriterion(criterionName, criterionDescription, minThreshold, maxThreshold, weight);
            var filterRule = new OpportunityFilterRule(
                Guid.NewGuid(),
                Id,
                ruleName,
                criterion,
                priority,
                createdBy);

            _filterRules.Add(filterRule);
        }

        /// <summary>
        /// Evaluates an opportunity against all active filter rules
        /// </summary>
        public decimal EvaluateOpportunity(Dictionary<string, decimal> opportunityMetrics)
        {
            var activeRules = _filterRules.Where(r => r.IsActive).OrderBy(r => r.Priority);
            var totalScore = 0m;
            var totalWeight = 0;

            foreach (var rule in activeRules)
            {
                if (opportunityMetrics.TryGetValue(rule.Criterion.Name, out var value))
                {
                    if (rule.Criterion.Evaluate(value))
                    {
                        totalScore += rule.Criterion.Weight;
                    }
                    totalWeight += rule.Criterion.Weight;
                }
            }

            return totalWeight > 0 ? (totalScore / totalWeight) * 100 : 0;
        }

        /// <summary>
        /// Calculates opportunity cost for pursuing a specific opportunity
        /// </summary>
        public decimal CalculateOpportunityCost(
            decimal opportunityValue,
            decimal requiredInvestment,
            List<decimal> alternativeValues)
        {
            if (!alternativeValues.Any())
                return 0;

            var bestAlternative = alternativeValues.Max();
            var netOpportunityValue = opportunityValue - requiredInvestment;
            
            return Math.Max(0, bestAlternative - netOpportunityValue);
        }

        /// <summary>
        /// Adds a strategic experiment
        /// </summary>
        public void AddStrategicExperiment(string experimentDescription)
        {
            if (string.IsNullOrWhiteSpace(experimentDescription))
                throw new ArgumentException("Experiment description cannot be empty");

            if (experimentDescription.Length > 1000)
                throw new ArgumentException("Experiment description is too long");

            _strategicExperiments.Add(experimentDescription.Trim());
        }

        /// <summary>
        /// Updates the focus protection level
        /// </summary>
        public void UpdateFocusLevel(FocusLevel newLevel, string reason = null)
        {
            var previousLevel = CurrentFocusLevel;
            CurrentFocusLevel = newLevel;

            // Log the focus level change
            if (!string.IsNullOrEmpty(reason))
            {
                AddStrategicExperiment($"Focus level changed from {previousLevel} to {newLevel}. Reason: {reason}");
            }
        }

        /// <summary>
        /// Activates a specific filter rule
        /// </summary>
        public void ActivateFilterRule(Guid ruleId)
        {
            var rule = _filterRules.FirstOrDefault(r => r.Id == ruleId);
            if (rule == null)
                throw new EntityNotFoundException(typeof(OpportunityFilterRule), ruleId);

            rule.Activate();
        }

        /// <summary>
        /// Deactivates a specific filter rule
        /// </summary>
        public void DeactivateFilterRule(Guid ruleId)
        {
            var rule = _filterRules.FirstOrDefault(r => r.Id == ruleId);
            if (rule == null)
                throw new EntityNotFoundException(typeof(OpportunityFilterRule), ruleId);

            rule.Deactivate();
        }

        /// <summary>
        /// Gets active filter rules
        /// </summary>
        public IEnumerable<OpportunityFilterRule> GetActiveFilterRules()
        {
            return _filterRules.Where(r => r.IsActive).OrderBy(r => r.Priority);
        }

        private void SetRecordName(string recordName)
        {
            RecordName = Check.NotNullOrWhiteSpace(recordName, nameof(recordName), maxLength: 200);
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(description, nameof(description), maxLength: 1000);
        }
    }
}
