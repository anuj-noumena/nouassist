namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Entities
{
    /// <summary>
    /// Entity representing engagement tracking with investor updates
    /// </summary>
    public class EngagementRecord : Entity<Guid>
    {
        public Guid UpdateId { get; private set; }
        public Guid InvestorId { get; private set; }
        public EngagementType Type { get; private set; }
        public DateTime EngagedAt { get; private set; }
        public string Details { get; private set; }
        public string IpAddress { get; private set; }
        public string UserAgent { get; private set; }

        protected EngagementRecord() { }

        internal EngagementRecord(
            Guid id,
            Guid updateId,
            Guid investorId,
            EngagementType type,
            DateTime engagedAt,
            string details = null,
            string ipAddress = null,
            string userAgent = null) : base(id)
        {
            UpdateId = updateId;
            InvestorId = investorId;
            Type = type;
            EngagedAt = engagedAt;
            Details = details;
            IpAddress = ipAddress;
            UserAgent = userAgent;
        }
    }
}
