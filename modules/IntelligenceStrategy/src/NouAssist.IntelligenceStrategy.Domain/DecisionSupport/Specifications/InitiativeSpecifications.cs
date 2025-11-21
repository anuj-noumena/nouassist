using System;
using System.Linq.Expressions;
using NouAssist.DecisionSupport.Aggregates;
using NouAssist.DecisionSupport.Enums;
using NouAssist.IntelligenceStrategy.DecisionSupport.Aggregates;
using NouAssist.IntelligenceStrategy.DecisionSupport.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.DecisionSupport.Specifications
{
    /// <summary>
    /// Base specification class for Strategic Initiative
    /// </summary>
    public abstract class InitiativeSpecification : Specification<StrategicInitiativeProposal>
    {
    }

    /// <summary>
    /// Specification for proposed initiatives
    /// </summary>
    public class ProposedInitiativeSpecification : InitiativeSpecification
    {
        public override Expression<Func<StrategicInitiativeProposal, bool>> ToExpression()
        {
            return initiative => initiative.Status == ProposalStatus.Proposed;
        }
    }

    /// <summary>
    /// Specification for approved initiatives
    /// </summary>
    public class ApprovedInitiativeSpecification : InitiativeSpecification
    {
        public override Expression<Func<StrategicInitiativeProposal, bool>> ToExpression()
        {
            return initiative => initiative.Status == ProposalStatus.Approved;
        }
    }

    /// <summary>
    /// Specification for initiatives linked to a decision model
    /// </summary>
    public class InitiativesByDecisionModelSpecification : InitiativeSpecification
    {
        private readonly Guid _decisionModelId;

        public InitiativesByDecisionModelSpecification(Guid decisionModelId)
        {
            _decisionModelId = decisionModelId;
        }

        public override Expression<Func<StrategicInitiativeProposal, bool>> ToExpression()
        {
            return initiative => initiative.DecisionModelId == _decisionModelId;
        }
    }
}
