namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Entities
{
    /// <summary>
    /// Represents an alternative considered for a decision
    /// </summary>
    public class DecisionAlternative : Entity
    {
        public Guid DecisionRecordId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string ProsCons { get; private set; }
        public bool IsChosen { get; private set; }

        protected DecisionAlternative() { }

        internal DecisionAlternative(
            Guid decisionRecordId, 
            string title, 
            string description, 
            string prosCons, 
            bool isChosen = false)
        {
            DecisionRecordId = decisionRecordId;
            
            Title = Check.NotNullOrWhiteSpace(
                title, 
                nameof(title), 
                maxLength: KnowledgeInnovationConsts.DefaultLengths.Title);
            
            Description = Check.NotNullOrWhiteSpace(
                description, 
                nameof(description), 
                maxLength: KnowledgeInnovationConsts.DecisionRecord.ContextMaxLength);
            
            ProsCons = prosCons;
            IsChosen = isChosen;
        }

        internal void Update(string title, string description, string prosCons)
        {
            Title = Check.NotNullOrWhiteSpace(
                title, 
                nameof(title), 
                maxLength: KnowledgeInnovationConsts.DefaultLengths.Title);
            
            Description = Check.NotNullOrWhiteSpace(
                description, 
                nameof(description), 
                maxLength: KnowledgeInnovationConsts.DecisionRecord.ContextMaxLength);
            
            ProsCons = prosCons;
        }

        internal void MarkAsChosen()
        {
            IsChosen = true;
        }

        internal void MarkAsNotChosen()
        {
            IsChosen = false;
        }

        public override object[] GetKeys()
        {
            return new object[] { DecisionRecordId, Title };
        }
    }
}
