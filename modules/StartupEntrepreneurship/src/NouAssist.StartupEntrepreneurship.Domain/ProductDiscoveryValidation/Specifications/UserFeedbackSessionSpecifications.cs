using System;
using System.Linq.Expressions;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Aggregates;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Enums;

namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Specifications
{
    /// <summary>
    /// Base specification class for UserFeedbackSession aggregate
    /// </summary>
    public abstract class UserFeedbackSessionSpecification : Specification<UserFeedbackSession>
    {
    }

    /// <summary>
    /// Specification for sessions with comprehensive feedback
    /// </summary>
    public class ComprehensiveFeedbackSpecification : UserFeedbackSessionSpecification
    {
        private readonly int _minimumFeedbackCount;
        private readonly decimal _minimumAverageRating;

        public ComprehensiveFeedbackSpecification(int minimumFeedbackCount = 5, decimal minimumAverageRating = 60m)
        {
            _minimumFeedbackCount = minimumFeedbackCount;
            _minimumAverageRating = minimumAverageRating;
        }

        public override Expression<Func<UserFeedbackSession, bool>> ToExpression()
        {
            return session => 
                session.Status == InterviewStatus.Completed &&
                session.FeedbackData.Count >= _minimumFeedbackCount;
        }
    }

    /// <summary>
    /// Specification for sessions related to a specific prototype
    /// </summary>
    public class PrototypeFeedbackSpecification : UserFeedbackSessionSpecification
    {
        private readonly Guid _prototypeId;

        public PrototypeFeedbackSpecification(Guid prototypeId)
        {
            _prototypeId = prototypeId;
        }

        public override Expression<Func<UserFeedbackSession, bool>> ToExpression()
        {
            return session => session.RelatedPrototypeId == _prototypeId;
        }
    }

    /// <summary>
    /// Specification for sessions with positive feedback
    /// </summary>
    public class PositiveFeedbackSpecification : UserFeedbackSessionSpecification
    {
        private readonly decimal _minimumRatingThreshold;

        public PositiveFeedbackSpecification(decimal minimumRatingThreshold = 70m)
        {
            _minimumRatingThreshold = minimumRatingThreshold;
        }

        public override Expression<Func<UserFeedbackSession, bool>> ToExpression()
        {
            // Note: This will need custom implementation in the repository
            // as we can't easily calculate average rating in LINQ to Entities
            return session => 
                session.Status == InterviewStatus.Completed &&
                session.FeedbackData.Any();
        }
    }

    /// <summary>
    /// Specification for sessions needing follow-up analysis
    /// </summary>
    public class NeedsAnalysisSpecification : UserFeedbackSessionSpecification
    {
        private readonly int _daysSinceCompletion;

        public NeedsAnalysisSpecification(int daysSinceCompletion = 7)
        {
            _daysSinceCompletion = daysSinceCompletion;
        }

        public override Expression<Func<UserFeedbackSession, bool>> ToExpression()
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-_daysSinceCompletion);
            return session => 
                session.Status == InterviewStatus.Completed &&
                session.CompletedDate.HasValue &&
                session.CompletedDate.Value <= cutoffDate &&
                session.FeedbackData.Any();
        }
    }
}
