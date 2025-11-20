namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Entities
{
    /// <summary>
    /// Represents a rule in the opportunity filtering system
    /// </summary>
    public class OpportunityFilterRule : Entity<Guid>
    {
        public Guid OpportunityManagementRecordId { get; private set; }
        public string RuleName { get; private set; }
        public FilterCriterion Criterion { get; private set; }
        public bool IsActive { get; private set; }
        public int Priority { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public string CreatedBy { get; private set; }

        protected OpportunityFilterRule() { }

        internal OpportunityFilterRule(
            Guid id,
            Guid opportunityManagementRecordId,
            string ruleName,
            FilterCriterion criterion,
            int priority,
            string createdBy) : base(id)
        {
            OpportunityManagementRecordId = opportunityManagementRecordId;
            SetRuleName(ruleName);
            Criterion = criterion;
            Priority = priority;
            CreatedBy = createdBy;
            CreatedDate = DateTime.UtcNow;
            IsActive = true;
        }

        internal void Activate()
        {
            IsActive = true;
        }

        internal void Deactivate()
        {
            IsActive = false;
        }

        internal void UpdatePriority(int newPriority)
        {
            Priority = newPriority;
        }

        private void SetRuleName(string ruleName)
        {
            RuleName = Check.NotNullOrWhiteSpace(ruleName, nameof(ruleName), maxLength: 100);
        }
    }
}
