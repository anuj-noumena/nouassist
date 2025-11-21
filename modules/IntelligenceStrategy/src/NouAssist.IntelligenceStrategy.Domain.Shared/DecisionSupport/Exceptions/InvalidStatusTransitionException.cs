namespace NouAssist.IntelligenceStrategy.DecisionSupport.Exceptions
{
    /// <summary>
    /// Exception thrown when attempting an invalid status transition
    /// </summary>
    public class InvalidStatusTransitionException : DecisionSupportDomainException
    {
        /// <summary>
        /// The current status
        /// </summary>
        public object CurrentStatus { get; }
        
        /// <summary>
        /// The target status
        /// </summary>
        public object TargetStatus { get; }

        /// <summary>
        /// Creates a new instance of <see cref="InvalidStatusTransitionException"/>
        /// </summary>
        /// <param name="entityType">The type of entity</param>
        /// <param name="currentStatus">The current status</param>
        /// <param name="targetStatus">The target status</param>
        public InvalidStatusTransitionException(string entityType, object currentStatus, object targetStatus)
            : base("DecisionSupport:ErrorMessage:InvalidStatusTransition")
        {
            CurrentStatus = currentStatus;
            TargetStatus = targetStatus;
            WithData("entityType", entityType);
            WithData("currentStatus", currentStatus);
            WithData("targetStatus", targetStatus);
        }
    }
}
