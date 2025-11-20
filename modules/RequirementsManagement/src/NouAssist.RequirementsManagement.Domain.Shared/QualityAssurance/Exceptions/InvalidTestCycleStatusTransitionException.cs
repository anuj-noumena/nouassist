namespace NouAssist.RequirementsManagement.QualityAssurance.Exceptions
{
    /// <summary>
    /// Exception thrown when an invalid test cycle status transition is attempted
    /// </summary>
    public class InvalidTestCycleStatusTransitionException : QADomainException
    {
        /// <summary>
        /// The current status of the test cycle
        /// </summary>
        public TestCycleStatus CurrentStatus { get; }
        
        /// <summary>
        /// The target status of the test cycle
        /// </summary>
        public TestCycleStatus TargetStatus { get; }

        /// <summary>
        /// Creates a new instance of InvalidTestCycleStatusTransitionException
        /// </summary>
        public InvalidTestCycleStatusTransitionException(TestCycleStatus currentStatus, TestCycleStatus targetStatus)
            : base("QA:ErrorMessage:InvalidTestCycleStatusTransition")
        {
            CurrentStatus = currentStatus;
            TargetStatus = targetStatus;
            WithData("currentStatus", currentStatus.ToString());
            WithData("targetStatus", targetStatus.ToString());
        }
    }
}
