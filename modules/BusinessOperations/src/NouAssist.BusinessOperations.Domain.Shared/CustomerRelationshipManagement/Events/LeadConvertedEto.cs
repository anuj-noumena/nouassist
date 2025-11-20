namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Events
{
    /// <summary>
    /// Event raised when a lead is converted to a contact
    /// </summary>
    [EventName("NouAssist.BusinessOperations.CustomerRelationshipManagement.LeadConverted")]
    public class LeadConvertedEto : CrmEtoBase
    {
        /// <summary>
        /// The ID of the newly created contact
        /// </summary>
        public Guid ContactId { get; set; }
        
        /// <summary>
        /// The ID of the organization if the contact was assigned to one
        /// </summary>
        public Guid? OrganizationId { get; set; }
        
        /// <summary>
        /// The name of the lead that was converted
        /// </summary>
        public string LeadName { get; set; }
        
        /// <summary>
        /// The time when the lead was converted
        /// </summary>
        public DateTime ConversionTime { get; set; }
    }
}
