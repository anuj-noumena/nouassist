namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Specifications
{
    /// <summary>
    /// Base specification class for CustomerSegment entity
    /// </summary>
    public abstract class CustomerSegmentSpecification : Specification<CustomerSegment>
    {
    }

    /// <summary>
    /// Specification for segments that have met their target sample size
    /// </summary>
    public class CompletedSegmentSpecification : CustomerSegmentSpecification
    {
        public override Expression<Func<CustomerSegment, bool>> ToExpression()
        {
            return segment => segment.ActualSampleSize >= segment.TargetSampleSize;
        }
    }

    /// <summary>
    /// Specification for segments with low completion rates
    /// </summary>
    public class LowCompletionSegmentSpecification : CustomerSegmentSpecification
    {
        private readonly decimal _maximumCompletionPercentage;

        public LowCompletionSegmentSpecification(decimal maximumCompletionPercentage = 50m)
        {
            _maximumCompletionPercentage = maximumCompletionPercentage;
        }

        public override Expression<Func<CustomerSegment, bool>> ToExpression()
        {
            return segment => 
                segment.TargetSampleSize > 0 &&
                (decimal)segment.ActualSampleSize / segment.TargetSampleSize * 100 <= _maximumCompletionPercentage;
        }
    }

    /// <summary>
    /// Specification for segments created within a specific time period
    /// </summary>
    public class RecentSegmentSpecification : CustomerSegmentSpecification
    {
        private readonly DateTime _startDate;

        public RecentSegmentSpecification(int daysAgo = 30)
        {
            _startDate = DateTime.UtcNow.AddDays(-daysAgo);
        }

        public override Expression<Func<CustomerSegment, bool>> ToExpression()
        {
            return segment => segment.CreationTime >= _startDate;
        }
    }
}
