namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Specifications
{
    /// <summary>
    /// Base specification class for InvestorUpdate aggregate
    /// </summary>
    public abstract class UpdateSpecification : Specification<InvestorUpdate>
    {
    }

    /// <summary>
    /// Specification for updates ready to be sent
    /// </summary>
    public class ReadyToSendUpdateSpecification : UpdateSpecification
    {
        public override Expression<Func<InvestorUpdate, bool>> ToExpression()
        {
            var now = DateTime.UtcNow;
            return update => update.Status == UpdateStatus.Scheduled &&
                            update.ScheduledDate <= now;
        }
    }

    /// <summary>
    /// Specification for updates with high engagement
    /// </summary>
    public class HighEngagementUpdateSpecification : UpdateSpecification
    {
        private readonly decimal _minimumEngagementRate;

        public HighEngagementUpdateSpecification(decimal minimumEngagementRate = 50)
        {
            _minimumEngagementRate = minimumEngagementRate;
        }

        public override Expression<Func<InvestorUpdate, bool>> ToExpression()
        {
            return update => update.Status == UpdateStatus.Sent &&
                            update.DistributionLogs.Count > 0 &&
                            (decimal)update.EngagementRecords.Select(e => e.InvestorId).Distinct().Count() /
                            update.DistributionLogs.Count * 100 >= _minimumEngagementRate;
        }
    }
}
