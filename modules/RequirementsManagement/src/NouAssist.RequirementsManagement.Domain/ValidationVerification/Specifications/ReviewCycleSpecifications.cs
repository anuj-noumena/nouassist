using System;
using System.Linq.Expressions;
using NouAssist.RequirementsManagement.ValidationVerification.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.RequirementsManagement.ValidationVerification.Specifications
{
    /// <summary>
    /// Base specification for RequirementReviewCycle
    /// </summary>
    public abstract class ReviewCycleSpecification : Specification<RequirementReviewCycle>
    {
    }

    /// <summary>
    /// Specification for active review cycles
    /// </summary>
    public class ActiveReviewCycleSpecification : ReviewCycleSpecification
    {
        public override Expression<Func<RequirementReviewCycle, bool>> ToExpression()
        {
            return cycle => cycle.Status == ReviewCycleStatus.InProgress;
        }
    }

    /// <summary>
    /// Specification for overdue review cycles
    /// </summary>
    public class OverdueReviewCycleSpecification : ReviewCycleSpecification
    {
        private readonly DateTime _currentTime;

        public OverdueReviewCycleSpecification(DateTime currentTime)
        {
            _currentTime = currentTime;
        }

        public override Expression<Func<RequirementReviewCycle, bool>> ToExpression()
        {
            return cycle => cycle.Status == ReviewCycleStatus.InProgress &&
                           cycle.TargetCompletionDate.HasValue &&
                           cycle.TargetCompletionDate.Value < _currentTime;
        }
    }

    /// <summary>
    /// Specification for review cycles assigned to a specific reviewer
    /// </summary>
    public class ReviewerAssignedSpecification : ReviewCycleSpecification
    {
        private readonly Guid _reviewerId;

        public ReviewerAssignedSpecification(Guid reviewerId)
        {
            _reviewerId = reviewerId;
        }

        public override Expression<Func<RequirementReviewCycle, bool>> ToExpression()
        {
            return cycle => cycle.ReviewerAssignments.Any(ra => ra.ReviewerId == _reviewerId);
        }
    }

    /// <summary>
    /// Specification for review cycles with pending reviews
    /// </summary>
    public class PendingReviewsSpecification : ReviewCycleSpecification
    {
        public override Expression<Func<RequirementReviewCycle, bool>> ToExpression()
        {
            return cycle => cycle.Status == ReviewCycleStatus.InProgress &&
                           cycle.ReviewerAssignments.Count(ra => ra.IsRequired) >
                           cycle.FeedbackItems.Count;
        }
    }

    /// <summary>
    /// Specification for review cycles within a specific date range
    /// </summary>
    public class ReviewCycleDateRangeSpecification : ReviewCycleSpecification
    {
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;

        public ReviewCycleDateRangeSpecification(DateTime startDate, DateTime endDate)
        {
            _startDate = startDate;
            _endDate = endDate;
        }

        public override Expression<Func<RequirementReviewCycle, bool>> ToExpression()
        {
            return cycle => cycle.CreationTime >= _startDate && cycle.CreationTime <= _endDate;
        }
    }
}
