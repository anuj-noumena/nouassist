using System;
using System.Linq.Expressions;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Enums;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Specifications
{
    /// <summary>
    /// Base specification class for Idea aggregate
    /// </summary>
    public abstract class IdeaSpecification : Specification<Idea>
    {
    }

    /// <summary>
    /// Specification for ideas in a specific status
    /// </summary>
    public class IdeaStatusSpecification : IdeaSpecification
    {
        private readonly IdeaStatus _status;

        public IdeaStatusSpecification(IdeaStatus status)
        {
            _status = status;
        }

        public override Expression<Func<Idea, bool>> ToExpression()
        {
            return idea => idea.Status == _status;
        }
    }

    /// <summary>
    /// Specification for ideas that can be evaluated
    /// </summary>
    public class EvaluableIdeaSpecification : IdeaSpecification
    {
        public override Expression<Func<Idea, bool>> ToExpression()
        {
            return idea => idea.Status == IdeaStatus.UnderEvaluation;
        }
    }

    /// <summary>
    /// Specification for ideas with high priority
    /// </summary>
    public class HighPriorityIdeaSpecification : IdeaSpecification
    {
        public override Expression<Func<Idea, bool>> ToExpression()
        {
            return idea => idea.Priority >= 8;
        }
    }

    /// <summary>
    /// Specification for ideas created by a specific user
    /// </summary>
    public class IdeaCreatedBySpecification : IdeaSpecification
    {
        private readonly Guid _creatorId;

        public IdeaCreatedBySpecification(Guid creatorId)
        {
            _creatorId = creatorId;
        }

        public override Expression<Func<Idea, bool>> ToExpression()
        {
            return idea => idea.CreatorId == _creatorId;
        }
    }
}
