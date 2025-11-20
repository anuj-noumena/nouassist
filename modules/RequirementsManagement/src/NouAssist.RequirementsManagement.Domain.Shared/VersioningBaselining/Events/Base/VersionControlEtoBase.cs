namespace NouAssist.RequirementsManagement.VersioningBaselining.Events.Base
{
    /// <summary>
    /// Base class for VersionControl module events
    /// </summary>
    [Serializable]
    public abstract class VersionControlEtoBase
    {
        /// <summary>
        /// The tenant ID associated with the event
        /// </summary>
        public Guid? TenantId { get; set; }
    }
}
