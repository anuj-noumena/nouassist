namespace NouAssist.RequirementsManagement.QualityAssurance.Exceptions
{
    /// <summary>
    /// Exception thrown when an invalid defect status transition is attempted
    /// </summary>
    public class InvalidDefectStatusTransitionException : QADomainException
    {
        /// <summary>
        /// The current status of the defect
        /// </summary>
        public DefectStatus CurrentStatus { get; }
        
        /// <summary>
        /// The target status of the defect
        /// </summary>
        public DefectStatus TargetStatus { get; }

        /// <summary>
        /// Creates a new instance of InvalidDefectStatusTransitionException
        /// </summary>
        public InvalidDefectStatusTransitionException(DefectStatus currentStatus, DefectStatus targetStatus)
            : base("QA:ErrorMessage:InvalidDefectStatusTransition")
        {
            CurrentStatus = currentStatus;
            TargetStatus = targetStatus;
            WithData("currentStatus", currentStatus.ToString());
            WithData("targetStatus", targetStatus.ToString());
        }
    }
}
