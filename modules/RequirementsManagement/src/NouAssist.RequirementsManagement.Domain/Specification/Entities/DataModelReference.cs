namespace NouAssist.RequirementsManagement.Specification.Entities
{
    /// <summary>
    /// Represents a data model reference for a requirement
    /// </summary>
    public class DataModelReference : Entity<Guid>
    {
        public Guid RequirementId { get; private set; }
        public DataFieldSpec FieldSpecification { get; private set; }

        protected DataModelReference() { }

        internal DataModelReference(
            Guid id,
            Guid requirementId,
            DataFieldSpec fieldSpecification) : base(id)
        {
            RequirementId = requirementId;
            FieldSpecification = fieldSpecification ?? throw new ArgumentNullException(nameof(fieldSpecification));
        }

        internal void UpdateSpecification(DataFieldSpec fieldSpecification)
        {
            FieldSpecification = fieldSpecification ?? throw new ArgumentNullException(nameof(fieldSpecification));
        }
    }
}
