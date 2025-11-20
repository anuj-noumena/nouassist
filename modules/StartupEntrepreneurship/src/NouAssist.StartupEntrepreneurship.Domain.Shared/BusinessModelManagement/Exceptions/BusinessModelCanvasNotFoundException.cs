namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Exceptions
{
    /// <summary>
    /// Exception thrown when a business model canvas is not found
    /// </summary>
    public class BusinessModelCanvasNotFoundException : BusinessModelManagementDomainException
    {
        public BusinessModelCanvasNotFoundException(Guid id)
            : base("BusinessModelManagement:ErrorMessage:CanvasNotFound")
        {
            WithData("canvasId", id);
        }
    }
}
