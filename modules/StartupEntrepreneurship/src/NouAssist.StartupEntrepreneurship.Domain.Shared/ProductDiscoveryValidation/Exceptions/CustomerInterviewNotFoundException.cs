namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Exceptions
{
    /// <summary>
    /// Exception thrown when a customer interview is not found
    /// </summary>
    public class CustomerInterviewNotFoundException : ProductDiscoveryDomainException
    {
        public CustomerInterviewNotFoundException(Guid interviewId)
            : base("ProductDiscovery:ErrorMessage:CustomerInterviewNotFound")
        {
            WithData("interviewId", interviewId);
        }
    }
}
