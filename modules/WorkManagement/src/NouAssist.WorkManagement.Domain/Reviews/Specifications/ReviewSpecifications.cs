using System;
using System.Linq.Expressions;
using NouAssist.WorkManagement.Reviews.Aggregates;
using NouAssist.WorkManagement.Reviews.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.WorkManagement.Reviews.Specifications
{
    public class IncompleteReviewsSpecification : Specification<Review>
    {
        public override Expression<Func<Review, bool>> ToExpression() =>
            r => !r.IsComplete;
    }

    public class CompletedReviewsSpecification : Specification<Review>
    {
        public override Expression<Func<Review, bool>> ToExpression() =>
            r => r.IsComplete;
    }

    public class ReviewsByTypeSpecification : Specification<Review>
    {
        private readonly ReviewType _reviewType;

        public ReviewsByTypeSpecification(ReviewType reviewType)
        {
            _reviewType = reviewType;
        }

        public override Expression<Func<Review, bool>> ToExpression() =>
            r => r.ReviewType == _reviewType;
    }

    public class ReviewsInPeriodSpecification : Specification<Review>
    {
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;

        public ReviewsInPeriodSpecification(DateTime startDate, DateTime endDate)
        {
            _startDate = startDate;
            _endDate = endDate;
        }

        public override Expression<Func<Review, bool>> ToExpression() =>
            r => r.CreationTime >= _startDate && r.CreationTime <= _endDate;
    }

    public class OverdueReviewsSpecification : Specification<Review>
    {
        public override Expression<Func<Review, bool>> ToExpression() =>
            r => !r.IsComplete && 
                 r.NextReviewDue.HasValue && 
                 r.NextReviewDue.Value < DateTime.Now;
    }

    public class HighQualityReviewsSpecification : Specification<Review>
    {
        public override Expression<Func<Review, bool>> ToExpression() =>
            r => r.IsComplete && 
                 r.CompletenessScore.HasValue && 
                 r.CompletenessScore.Value >= 80;
    }
}
