namespace NouAssist.RequirementsManagement.VersioningBaselining.Exceptions
{
    /// <summary>
    /// Exception thrown when attempting an invalid baseline status transition
    /// </summary>
    public class InvalidBaselineStatusTransitionException : VersionControlDomainException
    {
        /// <summary>
        /// ID of the baseline
        /// </summary>
        public Guid BaselineId { get; }
        
        /// <summary>
        /// Current status of the baseline
        /// </summary>
        public BaselineStatus CurrentStatus { get; }
        
        /// <summary>
        /// Target status that was attempted
        /// </summary>
        public BaselineStatus TargetStatus { get; }

        /// <summary>
        /// Creates a new InvalidBaselineStatusTransitionException
        /// </summary>
        public InvalidBaselineStatusTransitionException(Guid baselineId, BaselineStatus currentStatus, BaselineStatus targetStatus)
            : base("VersionControl:ErrorMessage:InvalidBaselineStatusTransition")
        {
            BaselineId = baselineId;
            CurrentStatus = currentStatus;
            TargetStatus = targetStatus;
            
            WithData("baselineId", baselineId);
            WithData("currentStatus", currentStatus);
            WithData("targetStatus", targetStatus);
        }
    }
}
