using System;
using System.Linq.Expressions;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Aggregates;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Specifications
{
    /// <summary>
    /// Base specification class for Crisis Management Plan aggregate
    /// </summary>
    public abstract class CrisisManagementSpecification : Specification<CrisisManagementPlan>
    {
    }

    /// <summary>
    /// Specification for active crisis management plans
    /// </summary>
    public class ActiveCrisisPlansSpecification : CrisisManagementSpecification
    {
        public override Expression<Func<CrisisManagementPlan, bool>> ToExpression()
        {
            return plan => plan.IsActive;
        }
    }

    /// <summary>
    /// Specification for crisis management plans due for review
    /// </summary>
    public class CrisisPlansDueForReviewSpecification : CrisisManagementSpecification
    {
        public override Expression<Func<CrisisManagementPlan, bool>> ToExpression()
        {
            var now = DateTime.UtcNow;
            return plan => plan.NextReviewDate <= now;
        }
    }

    /// <summary>
    /// Specification for crisis management plans by maximum crisis level
    /// </summary>
    public class CrisisPlansByCrisisLevelSpecification : CrisisManagementSpecification
    {
        private readonly CrisisLevel _level;

        public CrisisPlansByCrisisLevelSpecification(CrisisLevel level)
        {
            _level = level;
        }

        public override Expression<Func<CrisisManagementPlan, bool>> ToExpression()
        {
            return plan => plan.MaxCrisisLevel >= _level;
        }
    }
}
