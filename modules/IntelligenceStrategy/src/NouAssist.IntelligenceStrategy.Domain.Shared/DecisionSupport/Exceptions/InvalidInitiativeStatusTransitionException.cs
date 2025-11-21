using NouAssist.IntelligenceStrategy.DecisionSupport.Enums;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.Exceptions
{
    /// <summary>
    /// Exception thrown when attempting an invalid status transition for a strategic initiative
    /// </summary>
    public class InvalidInitiativeStatusTransitionException : DecisionSupportDomainException
    {
        /// <summary>
        /// Current status of the initiative
        /// </summary>
        public ProposalStatus CurrentStatus { get; }
        
        /// <summary>
        /// Target status that was attempted
        /// </summary>
        public ProposalStatus TargetStatus { get; }

        /// <summary>
        /// Creates a new instance of <see cref="InvalidInitiativeStatusTransitionException"/>
        /// </summary>
        /// <param name="currentStatus">Current status of the initiative</param>
        /// <param name="targetStatus">Target status that was attempted</param>
        public InvalidInitiativeStatusTransitionException(ProposalStatus currentStatus, ProposalStatus targetStatus)
            : base("DecisionSupport:ErrorMessage:InvalidStatusTransition")
        {
            CurrentStatus = currentStatus;
            TargetStatus = targetStatus;
            
            WithData("currentStatus", currentStatus.ToString());
            WithData("targetStatus", targetStatus.ToString());
        }
    }
}
