namespace NouAssist.StartupEntrepreneurship.LegalCompliance.Specifications
{
    /// <summary>
    /// Base specification for intellectual property assets
    /// </summary>
    public abstract class IntellectualPropertySpecification : Specification<IntellectualPropertyAsset>
    {
    }

    /// <summary>
    /// Specification for intellectual property assets with pending renewal
    /// </summary>
    public class PendingRenewalIPSpecification : IntellectualPropertySpecification
    {
        private readonly DateTime _referenceDate;
        private readonly int _monthsThreshold;

        public PendingRenewalIPSpecification(DateTime referenceDate, int monthsThreshold = 6)
        {
            _referenceDate = referenceDate;
            _monthsThreshold = monthsThreshold;
        }

        public override Expression<Func<IntellectualPropertyAsset, bool>> ToExpression()
        {
            return ip => ip.ExpirationDate.HasValue &&
                         ip.ExpirationDate.Value > _referenceDate &&
                         ip.ExpirationDate.Value < _referenceDate.AddMonths(_monthsThreshold) &&
                         ip.Status.Status != FilingStatusType.Expired &&
                         ip.Status.Status != FilingStatusType.Abandoned;
        }
    }

    /// <summary>
    /// Specification for intellectual property assets with approaching deadlines
    /// </summary>
    public class DeadlineApproachingIPSpecification : IntellectualPropertySpecification
    {
        private readonly DateTime _referenceDate;
        private readonly int _daysThreshold;

        public DeadlineApproachingIPSpecification(DateTime referenceDate, int daysThreshold = 30)
        {
            _referenceDate = referenceDate;
            _daysThreshold = daysThreshold;
        }

        public override Expression<Func<IntellectualPropertyAsset, bool>> ToExpression()
        {
            var thresholdDate = _referenceDate.AddDays(_daysThreshold);
            
            return ip => ip.FilingDeadlines.Any(fd => 
                !fd.IsCompleted && 
                fd.Deadline > _referenceDate && 
                fd.Deadline <= thresholdDate);
        }
    }

    /// <summary>
    /// Specification for active intellectual property assets
    /// </summary>
    public class ActiveIPSpecification : IntellectualPropertySpecification
    {
        public override Expression<Func<IntellectualPropertyAsset, bool>> ToExpression()
        {
            return ip => ip.Status.Status != FilingStatusType.Expired &&
                         ip.Status.Status != FilingStatusType.Abandoned &&
                         ip.Status.Status != FilingStatusType.Rejected;
        }
    }
}
