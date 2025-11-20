/// <summary>
    /// Data transfer object for lead conversion result
    /// </summary>
    public class LeadConversionResultDto
    {
        /// <summary>
        /// The ID of the converted lead
        /// </summary>
        public Guid LeadId { get; set; }
        
        /// <summary>
        /// The ID of the newly created contact
        /// </summary>
        public Guid ContactId { get; set; }
        
        /// <summary>
        /// The name of the newly created contact
        /// </summary>
        public string ContactName { get; set; }
        
        /// <summary>
        /// The organization ID if assigned
        /// </summary>
        public Guid? OrganizationId { get; set; }
        
        /// <summary>
        /// The organization name if assigned
        /// </summary>
        public string OrganizationName { get; set; }
    }
}
