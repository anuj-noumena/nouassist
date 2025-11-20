namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Entities
{
    /// <summary>
    /// Entity representing distribution log for investor updates
    /// </summary>
    public class DistributionLog : Entity<Guid>
    {
        public Guid UpdateId { get; private set; }
        public Guid InvestorId { get; private set; }
        public UpdateStatus Status { get; private set; }
        public DateTime AttemptedAt { get; private set; }
        public DateTime? DeliveredAt { get; private set; }
        public string ErrorMessage { get; private set; }
        public string Channel { get; private set; }

        protected DistributionLog() { }

        internal DistributionLog(
            Guid id,
            Guid updateId,
            Guid investorId,
            string channel) : base(id)
        {
            UpdateId = updateId;
            InvestorId = investorId;
            Status = UpdateStatus.Scheduled;
            AttemptedAt = DateTime.UtcNow;
            Channel = channel;
        }

        public void MarkAsSent()
        {
            Status = UpdateStatus.Sent;
            ErrorMessage = null;
        }

        public void MarkAsDelivered(DateTime deliveredAt)
        {
            Status = UpdateStatus.Delivered;
            DeliveredAt = deliveredAt;
            ErrorMessage = null;
        }

        public void MarkAsRead()
        {
            Status = UpdateStatus.Read;
        }

        public void MarkAsFailed(string errorMessage)
        {
            Status = UpdateStatus.Failed;
            ErrorMessage = errorMessage;
        }
    }
}
