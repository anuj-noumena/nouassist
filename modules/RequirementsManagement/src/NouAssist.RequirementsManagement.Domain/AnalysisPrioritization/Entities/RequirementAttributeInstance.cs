namespace NouAssist.RequirementsManagement.AnalysisPrioritization.Entities
{
    /// <summary>
    /// Represents an attribute instance assigned to an analyzed requirement
    /// </summary>
    public class RequirementAttributeInstance : Entity<Guid>
    {
        public Guid AnalyzedRequirementId { get; private set; }
        public string AttributeName { get; private set; }
        public AttributeType AttributeType { get; private set; }
        public AttributeValue AttributeValue { get; private set; }
        public DateTime CreatedTime { get; private set; }
        public Guid CreatedBy { get; private set; }

        protected RequirementAttributeInstance() { }

        internal RequirementAttributeInstance(
            Guid id,
            Guid analyzedRequirementId,
            string attributeName,
            AttributeType attributeType,
            AttributeValue attributeValue,
            Guid createdBy,
            DateTime createdTime)
            : base(id)
        {
            AnalyzedRequirementId = analyzedRequirementId;
            SetAttributeName(attributeName);
            AttributeType = attributeType;
            AttributeValue = Check.NotNull(attributeValue, nameof(attributeValue));
            CreatedBy = createdBy;
            CreatedTime = createdTime;
        }

        internal void UpdateValue(AttributeValue newValue)
        {
            AttributeValue = Check.NotNull(newValue, nameof(newValue));
        }

        private void SetAttributeName(string attributeName)
        {
            AttributeName = Check.NotNullOrWhiteSpace(
                attributeName,
                nameof(attributeName),
                maxLength: AnalysisConsts.MaxAttributeNameLength
            );
        }
    }
}
