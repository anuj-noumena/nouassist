using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Performance.Dtos
{
    /// <summary>
    /// DTO for getting performance snapshots
    /// </summary>
    public class GetPerformanceSnapshotsDto : PagedResultRequestDto
    {
        /// <summary>
        /// User ID
        /// </summary>
        [Required]
        public Guid UserId { get; set; }
        
        /// <summary>
        /// Filter by snapshot type
        /// </summary>
        public string SnapshotType { get; set; }
        
        /// <summary>
        /// Filter by start date
        /// </summary>
        public DateTime? StartDate { get; set; }
        
        /// <summary>
        /// Filter by end date
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
}
