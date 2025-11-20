namespace NouAssist.BusinessOperations.FreelanceOperations.Events.Base
{
    /// <summary>
    /// Base event class for client-related events
    /// </summary>
    [Serializable]
    public abstract class ClientEtoBase
    {
        /// <summary>
        /// ID of the client
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Name of the client
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Email of the client
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// Current status of the client
        /// </summary>
        public ClientStatus Status { get; set; }
        
        /// <summary>
        /// Tenant ID for multi-tenancy support
        /// </summary>
        public Guid? TenantId { get; set; }
    }
}
