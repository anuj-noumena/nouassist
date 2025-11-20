namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Exceptions
{
    /// <summary>
    /// Exception thrown when attempting to create a duplicate version
    /// </summary>
    public class DuplicateVersionException : BusinessModelManagementDomainException
    {
        public DuplicateVersionException(string versionName)
            : base("BusinessModelManagement:ErrorMessage:DuplicateVersion")
        {
            WithData("versionName", versionName);
        }
    }
}
