namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Exceptions
{
    /// <summary>
    /// Exception thrown when a canvas version is not found
    /// </summary>
    public class CanvasVersionNotFoundException : BusinessModelManagementDomainException
    {
        public CanvasVersionNotFoundException(Guid canvasId, int versionNumber)
            : base("BusinessModelManagement:ErrorMessage:VersionNotFound")
        {
            WithData("canvasId", canvasId);
            WithData("versionNumber", versionNumber);
        }
    }
}
