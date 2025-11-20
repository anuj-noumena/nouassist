namespace NouAssist.RequirementsManagement.Specification.Entities
{
    /// <summary>
    /// Represents a constraint associated with a requirement
    /// </summary>
    public class AssociatedConstraint : Entity<Guid>
    {
        public Guid RequirementId { get; private set; }
        public ConstraintStatement Constraint { get; private set; }

        protected AssociatedConstraint() { }

        internal AssociatedConstraint(
            Guid id,
            Guid requirementId,
            ConstraintStatement constraint) : base(id)
        {
            RequirementId = requirementId;
            Constraint = constraint ?? throw new ArgumentNullException(nameof(constraint));
        }

        internal void UpdateConstraint(ConstraintStatement constraint)
        {
            Constraint = constraint ?? throw new ArgumentNullException(nameof(constraint));
        }
    }
}
