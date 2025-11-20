namespace NouAssist.IntelligenceStrategy.DecisionSupport.Events.Base
{
    /// <summary>
    /// Base class for Strategic Initiative events
    /// </summary>
    [Serializable]
    public abstract class StrategicInitiativeEtoBase
    {
        /// <summary>
        /// ID of the strategic initiative
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Title of the strategic initiative
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// The tenant ID if multi-tenancy is enabled
        /// </summary>
        public Guid? TenantId { get; set; }
    }
}
