using NouAssist.RequirementsManagement.QualityAssurance.Enums;

namespace NouAssist.RequirementsManagement.QualityAssurance.Exceptions
{
    /// <summary>
    /// Exception thrown when an invalid test plan status transition is attempted
    /// </summary>
    public class InvalidTestPlanStatusTransitionException : QADomainException
    {
        /// <summary>
        /// The current status of the test plan
        /// </summary>
        public TestPlanStatus CurrentStatus { get; }
        
        /// <summary>
        /// The target status of the test plan
        /// </summary>
        public TestPlanStatus TargetStatus { get; }

        /// <summary>
        /// Creates a new instance of InvalidTestPlanStatusTransitionException
        /// </summary>
        public InvalidTestPlanStatusTransitionException(TestPlanStatus currentStatus, TestPlanStatus targetStatus)
            : base("QA:ErrorMessage:InvalidTestPlanStatusTransition")
        {
            CurrentStatus = currentStatus;
            TargetStatus = targetStatus;
            WithData("currentStatus", currentStatus.ToString());
            WithData("targetStatus", targetStatus.ToString());
        }
    }
}
