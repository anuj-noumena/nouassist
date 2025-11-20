namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Specifications
{
    /// <summary>
    /// Base specification class for LearningResource aggregate
    /// </summary>
    public abstract class LearningResourceSpecification : Specification<LearningResource>
    {
    }

    /// <summary>
    /// Specification for published learning resources
    /// </summary>
    public class PublishedLearningResourceSpecification : LearningResourceSpecification
    {
        public override Expression<Func<LearningResource, bool>> ToExpression()
        {
            return resource => resource.IsPublished;
        }
    }

    /// <summary>
    /// Specification for featured learning resources
    /// </summary>
    public class FeaturedLearningResourceSpecification : LearningResourceSpecification
    {
        public override Expression<Func<LearningResource, bool>> ToExpression()
        {
            return resource => resource.IsPublished && resource.IsFeatured;
        }
    }

    /// <summary>
    /// Specification for learning resources by type
    /// </summary>
    public class LearningResourceTypeSpecification : LearningResourceSpecification
    {
        private readonly ResourceType _resourceType;

        public LearningResourceTypeSpecification(ResourceType resourceType)
        {
            _resourceType = resourceType;
        }

        public override Expression<Func<LearningResource, bool>> ToExpression()
        {
            return resource => resource.ResourceType == _resourceType;
        }
    }

    /// <summary>
    /// Specification for learning resources completed by a specific user
    /// </summary>
    public class CompletedByUserSpecification : LearningResourceSpecification
    {
        private readonly Guid _userId;

        public CompletedByUserSpecification(Guid userId)
        {
            _userId = userId;
        }

        public override Expression<Func<LearningResource, bool>> ToExpression()
        {
            return resource => resource.UserProgressRecords.Any(
                p => p.UserId == _userId && p.Status == CompletionStatus.Completed);
        }
    }
}
