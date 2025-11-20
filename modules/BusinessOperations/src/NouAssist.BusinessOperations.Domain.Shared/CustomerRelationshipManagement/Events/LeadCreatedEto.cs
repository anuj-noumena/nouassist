namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Events
{
    /// <summary>
    /// Event raised when a new lead is created
    /// </summary>
    [EventName("NouAssist.BusinessOperations.CustomerRelationshipManagement.LeadCreated")]
    public class LeadCreatedEto : CrmEtoBase
    {
        /// <summary>
        /// The name of the lead
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The email of the lead
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// The source of the lead
        /// </summary>
        public LeadSource Source { get; set; }
        
        /// <summary>
        /// The creation time of the lead
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}
