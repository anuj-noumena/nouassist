namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Exceptions
{
    /// <summary>
    /// Exception thrown when an invalid interview status transition is attempted
    /// </summary>
    public class InvalidInterviewStatusTransitionException : ProductDiscoveryDomainException
    {
        public InvalidInterviewStatusTransitionException(InterviewStatus currentStatus, InterviewStatus targetStatus)
            : base("ProductDiscovery:ErrorMessage:InvalidStatusTransition")
        {
            WithData("currentStatus", currentStatus);
            WithData("targetStatus", targetStatus);
        }
    }
}
