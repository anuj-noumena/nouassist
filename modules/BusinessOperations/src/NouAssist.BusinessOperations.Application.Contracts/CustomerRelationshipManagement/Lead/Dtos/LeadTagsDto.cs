/// <summary>
    /// Data transfer object for managing lead tags
    /// </summary>
    public class LeadTagsDto
    {
        /// <summary>
        /// Tags to add or remove
        /// </summary>
        [Required]
        public List<string> Tags { get; set; }
    }
