namespace NouAssist.RequirementsManagement.AnalysisPrioritization.Entities
{
    /// <summary>
    /// Represents a relationship link between two requirements
    /// </summary>
    public class RelationshipLink : Entity<Guid>
    {
        public Guid SourceRequirementId { get; private set; }
        public Guid TargetRequirementId { get; private set; }
        public RelationshipType RelationshipType { get; private set; }
        public string? Description { get; private set; }
        public DateTime CreatedTime { get; private set; }
        public Guid CreatedBy { get; private set; }

        protected RelationshipLink() { }

        internal RelationshipLink(
            Guid id,
            Guid sourceRequirementId,
            Guid targetRequirementId,
            RelationshipType relationshipType,
            string? description,
            Guid createdBy,
            DateTime createdTime)
            : base(id)
        {
            if (sourceRequirementId == targetRequirementId)
            {
                throw new BusinessException("RequirementsAnalysisTools:ErrorMessage:SelfReference");
            }

            SourceRequirementId = sourceRequirementId;
            TargetRequirementId = targetRequirementId;
            RelationshipType = relationshipType;
            SetDescription(description);
            CreatedBy = createdBy;
            CreatedTime = createdTime;
        }

        internal void UpdateRelationshipType(RelationshipType newType)
        {
            RelationshipType = newType;
        }

        internal void UpdateDescription(string? newDescription)
        {
            SetDescription(newDescription);
        }

        private void SetDescription(string? description)
        {
            if (!string.IsNullOrWhiteSpace(description))
            {
                Description = Check.Length(
                    description,
                    nameof(description),
                    maxLength: AnalysisConsts.MaxRelationshipDescriptionLength
                );
            }
            else
            {
                Description = description;
            }
        }

        public RelationshipLink CreateInverse()
        {
            var inverseType = GetInverseRelationshipType();
            return new RelationshipLink(
                GuidGenerator.Create(),
                TargetRequirementId,
                SourceRequirementId,
                inverseType,
                Description,
                CreatedBy,
                CreatedTime
            );
        }

        private RelationshipType GetInverseRelationshipType()
        {
            return RelationshipType switch
            {
                RelationshipType.Dependency => RelationshipType.Dependency,
                RelationshipType.Parent_Child => RelationshipType.Parent_Child,
                RelationshipType.Derives_From => RelationshipType.Derives_From,
                _ => RelationshipType
            };
        }
    }
}
