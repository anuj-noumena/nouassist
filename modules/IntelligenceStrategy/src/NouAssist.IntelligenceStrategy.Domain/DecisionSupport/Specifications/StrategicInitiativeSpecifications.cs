using System;
using System.Linq.Expressions;
using NouAssist.IntelligenceStrategy.DecisionSupport.Aggregates;
using NouAssist.IntelligenceStrategy.DecisionSupport.Enums;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.Specifications
{
    /// <summary>
    /// Base specification for strategic initiatives
    /// </summary>
    public abstract class StrategicInitiativeSpecification : Specification<StrategicInitiativeProposal>
    {
    }

    /// <summary>
    /// Specification that identifies initiatives with high strategic alignment
    /// </summary>
    public class HighAlignmentInitiativeSpecification : StrategicInitiativeSpecification
    {
        /// <inheritdoc/>
        public override Expression<Func<StrategicInitiativeProposal, bool>> ToExpression()
        {
            return initiative => initiative.StrategicAlignment == AlignmentStatus.High;
        }
    }

    /// <summary>
    /// Specification that identifies initiatives in Proposed status
    /// </summary>
    public class ProposedInitiativeSpecification : StrategicInitiativeSpecification
    {
        /// <inheritdoc/>
        public override Expression<Func<StrategicInitiativeProposal, bool>> ToExpression()
        {
            return initiative => initiative.Status == ProposalStatus.Proposed;
        }
    }

    /// <summary>
    /// Specification that identifies initiatives derived from a specific decision model
    /// </summary>
    public class DecisionModelDerivedInitiativeSpecification : StrategicInitiativeSpecification
    {
        private readonly Guid _decisionModelId;

        /// <summary>
        /// Creates a new instance of <see cref="DecisionModelDerivedInitiativeSpecification"/>
        /// </summary>
        public DecisionModelDerivedInitiativeSpecification(Guid decisionModelId)
        {
            _decisionModelId = decisionModelId;
        }

        /// <inheritdoc/>
        public override Expression<Func<StrategicInitiativeProposal, bool>> ToExpression()
        {
            return initiative => initiative.RelatedDecisionModelId == _decisionModelId;
        }
    }

    /// <summary>
    /// Specification that identifies approved initiatives that need project creation
    /// </summary>
    public class ApprovedWithoutProjectSpecification : StrategicInitiativeSpecification
    {
        /// <inheritdoc/>
        public override Expression<Func<StrategicInitiativeProposal, bool>> ToExpression()
        {
            return initiative => 
                initiative.Status == ProposalStatus.Approved && 
                initiative.ProjectId == null;
        }
    }
}
