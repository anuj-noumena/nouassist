namespace NouAssist.RequirementsManagement.QualityAssurance.Events.Base
{
    /// <summary>
    /// Base class for all QA domain event transfer objects
    /// </summary>
    [Serializable]
    public abstract class QAEtoBase
    {
        /// <summary>
        /// The identifier of the entity that this event relates to
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// The tenant ID that this event belongs to
        /// </summary>
        public Guid? TenantId { get; set; }
    }
}
