namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Specifications
{
    /// <summary>
    /// Base specification class for CustomerInterview aggregate
    /// </summary>
    public abstract class CustomerInterviewSpecification : Specification<CustomerInterview>
    {
    }

    /// <summary>
    /// Specification for completed interviews with high-value insights
    /// </summary>
    public class HighValueInterviewSpecification : CustomerInterviewSpecification
    {
        private readonly int _minimumInsightCount;
        private readonly int _minimumHighImportanceInsights;

        public HighValueInterviewSpecification(int minimumInsightCount = 3, int minimumHighImportanceInsights = 1)
        {
            _minimumInsightCount = minimumInsightCount;
            _minimumHighImportanceInsights = minimumHighImportanceInsights;
        }

        public override Expression<Func<CustomerInterview, bool>> ToExpression()
        {
            return interview => 
                interview.Status == InterviewStatus.Completed &&
                interview.Insights.Count >= _minimumInsightCount &&
                interview.Insights.Count(i => i.Importance >= 4) >= _minimumHighImportanceInsights;
        }
    }

    /// <summary>
    /// Specification for interviews within a specific customer segment
    /// </summary>
    public class InterviewInSegmentSpecification : CustomerInterviewSpecification
    {
        private readonly Guid _customerSegmentId;

        public InterviewInSegmentSpecification(Guid customerSegmentId)
        {
            _customerSegmentId = customerSegmentId;
        }

        public override Expression<Func<CustomerInterview, bool>> ToExpression()
        {
            return interview => interview.CustomerSegmentId == _customerSegmentId;
        }
    }

    /// <summary>
    /// Specification for interviews scheduled within a date range
    /// </summary>
    public class InterviewInDateRangeSpecification : CustomerInterviewSpecification
    {
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;

        public InterviewInDateRangeSpecification(DateTime startDate, DateTime endDate)
        {
            _startDate = startDate;
            _endDate = endDate;
        }

        public override Expression<Func<CustomerInterview, bool>> ToExpression()
        {
            return interview => 
                interview.ScheduledDate >= _startDate &&
                interview.ScheduledDate <= _endDate;
        }
    }

    /// <summary>
    /// Specification for interviews that need follow-up
    /// </summary>
    public class InterviewNeedingFollowUpSpecification : CustomerInterviewSpecification
    {
        private readonly int _daysSinceCompletion;

        public InterviewNeedingFollowUpSpecification(int daysSinceCompletion = 30)
        {
            _daysSinceCompletion = daysSinceCompletion;
        }

        public override Expression<Func<CustomerInterview, bool>> ToExpression()
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-_daysSinceCompletion);
            return interview => 
                interview.Status == InterviewStatus.Completed &&
                interview.CompletedDate.HasValue &&
                interview.CompletedDate.Value <= cutoffDate &&
                interview.Insights.Any(i => i.Importance >= 4);
        }
    }
}
