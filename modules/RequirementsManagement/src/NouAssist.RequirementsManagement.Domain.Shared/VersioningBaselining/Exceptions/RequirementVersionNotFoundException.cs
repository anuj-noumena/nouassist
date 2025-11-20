namespace NouAssist.RequirementsManagement.VersioningBaselining.Exceptions
{
    /// <summary>
    /// Exception thrown when a specific requirement version cannot be found
    /// </summary>
    public class RequirementVersionNotFoundException : VersionControlDomainException
    {
        /// <summary>
        /// ID of the requirement
        /// </summary>
        public Guid RequirementId { get; }
        
        /// <summary>
        /// Version identifier of the requested version
        /// </summary>
        public string Version { get; }

        /// <summary>
        /// Creates a new RequirementVersionNotFoundException
        /// </summary>
        public RequirementVersionNotFoundException(Guid requirementId, string version)
            : base("VersionControl:ErrorMessage:RequirementVersionNotFound")
        {
            RequirementId = requirementId;
            Version = version;
            
            WithData("requirementId", requirementId);
            WithData("version", version);
        }
    }
}
