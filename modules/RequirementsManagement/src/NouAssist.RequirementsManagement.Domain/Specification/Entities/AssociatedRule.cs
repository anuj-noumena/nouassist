namespace NouAssist.RequirementsManagement.Specification.Entities
{
    /// <summary>
    /// Represents a business rule associated with a requirement
    /// </summary>
    public class AssociatedRule : Entity<Guid>
    {
        public Guid RequirementId { get; private set; }
        public RuleDescription Rule { get; private set; }

        protected AssociatedRule() { }

        internal AssociatedRule(
            Guid id,
            Guid requirementId,
            RuleDescription rule) : base(id)
        {
            RequirementId = requirementId;
            Rule = rule ?? throw new ArgumentNullException(nameof(rule));
        }

        internal void UpdateRule(RuleDescription rule)
        {
            Rule = rule ?? throw new ArgumentNullException(nameof(rule));
        }
    }
}
