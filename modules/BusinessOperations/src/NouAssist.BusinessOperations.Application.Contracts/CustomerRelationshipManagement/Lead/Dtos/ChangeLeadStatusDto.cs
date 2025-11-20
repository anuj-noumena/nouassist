/// <summary>
    /// Data transfer object for changing lead status
    /// </summary>
    public class ChangeLeadStatusDto
    {
        /// <summary>
        /// New status to set
        /// </summary>
        [Required]
        public LeadStatus Status { get; set; }
    }
