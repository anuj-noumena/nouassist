namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Entities
{
    /// <summary>
    /// Entity representing an interaction with a prospect
    /// </summary>
    public class InteractionLog : Entity<Guid>, IHasCreationTime
    {
        public Guid ProspectId { get; private set; }
        public InteractionType Type { get; private set; }
        public string Summary { get; private set; }
        public string Notes { get; private set; }
        public DateTime InteractionDate { get; private set; }
        public DateTime CreationTime { get; set; }
        
        protected InteractionLog() { }
        
        internal InteractionLog(
            Guid id,
            Guid prospectId,
            InteractionType type,
            string summary,
            DateTime interactionDate,
            string notes = null) : base(id)
        {
            ProspectId = prospectId;
            Type = type;
            SetSummary(summary);
            InteractionDate = interactionDate;
            Notes = notes;
            CreationTime = DateTime.Now;
        }
        
        public void UpdateDetails(
            InteractionType type,
            string summary,
            DateTime interactionDate,
            string notes = null)
        {
            Type = type;
            SetSummary(summary);
            InteractionDate = interactionDate;
            Notes = notes;
        }
        
        private void SetSummary(string summary)
        {
            if (string.IsNullOrWhiteSpace(summary))
            {
                throw new ArgumentException("Summary cannot be null or empty.");
            }
            
            Summary = summary;
        }
    }
}
