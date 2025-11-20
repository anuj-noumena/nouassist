namespace NouAssist.BusinessOperations.FreelanceOperations.Events.Base
{
    /// <summary>
    /// Base event class for project-related events
    /// </summary>
    [Serializable]
    public abstract class ProjectEtoBase
    {
        /// <summary>
        /// ID of the project
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Name of the project
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// ID of the client associated with this project
        /// </summary>
        public Guid ClientId { get; set; }
        
        /// <summary>
        /// Current status of the project
        /// </summary>
        public ProjectStatus Status { get; set; }
        
        /// <summary>
        /// Billing type for this project
        /// </summary>
        public BillingType BillingType { get; set; }
        
        /// <summary>
        /// Tenant ID for multi-tenancy support
        /// </summary>
        public Guid? TenantId { get; set; }
    }
}
