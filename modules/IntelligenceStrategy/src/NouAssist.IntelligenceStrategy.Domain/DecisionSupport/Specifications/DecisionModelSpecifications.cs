using System;
using System.Linq.Expressions;
using NouAssist.IntelligenceStrategy.DecisionSupport.Aggregates;
using Volo.Abp.Specifications;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.Specifications
{
    /// <summary>
    /// Base specification class for Decision Model
    /// </summary>
    public abstract class DecisionModelSpecification : Specification<DecisionModelInstance>
    {
    }

    /// <summary>
    /// Specification for decision models with pending decisions
    /// </summary>
    public class PendingDecisionModelSpecification : DecisionModelSpecification
    {
        private readonly DateTime _cutoffDate;

        public PendingDecisionModelSpecification(DateTime cutoffDate)
        {
            _cutoffDate = cutoffDate;
        }

        public override Expression<Func<DecisionModelInstance, bool>> ToExpression()
        {
            return model => model.DecisionDueDate.HasValue && 
                           model.DecisionDueDate.Value <= _cutoffDate;
        }
    }

    /// <summary>
    /// Specification for decision models with multiple alternatives
    /// </summary>
    public class MultipleAlternativesSpecification : DecisionModelSpecification
    {
        private readonly int _minimumCount;

        public MultipleAlternativesSpecification(int minimumCount = 2)
        {
            _minimumCount = minimumCount;
        }

        public override Expression<Func<DecisionModelInstance, bool>> ToExpression()
        {
            return model => model.Alternatives.Count >= _minimumCount;
        }
    }
}
