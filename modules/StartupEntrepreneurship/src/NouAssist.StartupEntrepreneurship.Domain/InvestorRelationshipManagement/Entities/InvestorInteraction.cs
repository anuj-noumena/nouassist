namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Entities
{
    /// <summary>
    /// Entity representing an interaction with an investor
    /// </summary>
    public class InvestorInteraction : FullAuditedEntity<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public Guid InvestorId { get; private set; }
        public InteractionType Type { get; private set; }
        public string Subject { get; private set; }
        public string Notes { get; private set; }
        public DateTime InteractionDate { get; private set; }
        public string Outcome { get; private set; }
        public Guid? RelatedEntityId { get; private set; }
        public string RelatedEntityType { get; private set; }

        protected InvestorInteraction() { }

        internal InvestorInteraction(
            Guid id,
            Guid investorId,
            InteractionType type,
            string subject,
            DateTime interactionDate,
            string notes = null,
            string outcome = null,
            Guid? relatedEntityId = null,
            string relatedEntityType = null,
            Guid? tenantId = null) : base(id)
        {
            InvestorId = investorId;
            Type = type;
            Subject = Check.NotNullOrWhiteSpace(subject, nameof(subject));
            InteractionDate = interactionDate;
            Notes = notes;
            Outcome = outcome;
            RelatedEntityId = relatedEntityId;
            RelatedEntityType = relatedEntityType;
            TenantId = tenantId;
        }

        public void UpdateNotes(string notes)
        {
            Notes = notes;
        }

        public void UpdateOutcome(string outcome)
        {
            Outcome = outcome;
        }
    }
}
