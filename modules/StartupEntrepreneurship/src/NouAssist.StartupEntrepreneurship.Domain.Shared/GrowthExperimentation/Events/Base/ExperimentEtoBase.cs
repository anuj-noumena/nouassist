namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Events.Base
{
    /// <summary>
    /// Base class for experiment-related event transfer objects
    /// </summary>
    [Serializable]
    public abstract class ExperimentEtoBase
    {
        /// <summary>
        /// The unique identifier of the experiment
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// The name of the experiment
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Tenant ID for multi-tenancy support
        /// </summary>
        public Guid? TenantId { get; set; }
    }
}
