using System;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Enums;
using Volo.Abp.Application.Dtos;

/// <summary>
    /// Data transfer object for lead list filtering and pagination
    /// </summary>
    public class GetLeadListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// Text to search in lead name, email, and company
        /// </summary>
        public string Filter { get; set; }
        
        /// <summary>
        /// Filter by lead status
        /// </summary>
        public LeadStatus? Status { get; set; }
        
        /// <summary>
        /// Filter by minimum score
        /// </summary>
        public int? MinScore { get; set; }
        
        /// <summary>
        /// Filter by lead source
        /// </summary>
        public LeadSource? Source { get; set; }
        
        /// <summary>
        /// Filter by tags (comma-separated)
        /// </summary>
        public string Tags { get; set; }
        
        /// <summary>
        /// Filter by creation date range start
        /// </summary>
        public DateTime? CreatedFrom { get; set; }
        
        /// <summary>
        /// Filter by creation date range end
        /// </summary>
        public DateTime? CreatedTo { get; set; }
    }
