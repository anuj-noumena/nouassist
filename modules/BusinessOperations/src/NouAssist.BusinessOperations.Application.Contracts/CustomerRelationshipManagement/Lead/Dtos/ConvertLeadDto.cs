/// <summary>
    /// Data transfer object for converting a lead to a contact
    /// </summary>
    public class ConvertLeadDto
    {
        /// <summary>
        /// Organization ID to assign the new contact to (optional)
        /// </summary>
        public Guid? OrganizationId { get; set; }
        
        /// <summary>
        /// Whether the new contact should be a primary contact for the organization
        /// </summary>
        public bool IsPrimaryContact { get; set; }
    }
