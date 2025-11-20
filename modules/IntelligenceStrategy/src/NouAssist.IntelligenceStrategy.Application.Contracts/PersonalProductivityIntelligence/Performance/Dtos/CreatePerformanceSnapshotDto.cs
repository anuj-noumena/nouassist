using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Performance.Dtos
{
    /// <summary>
    /// DTO for creating a performance snapshot
    /// </summary>
    public class CreatePerformanceSnapshotDto
    {
        /// <summary>
        /// User ID
        /// </summary>
        [Required]
        public Guid UserId { get; set; }
        
        /// <summary>
        /// Type of snapshot (Daily, Weekly, Monthly, etc.)
        /// </summary>
        [Required]
        [StringLength(50)]
        public string SnapshotType { get; set; }
        
        /// <summary>
        /// Start date of the period
        /// </summary>
        [Required]
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// End date of the period
        /// </summary>
        [Required]
        public DateTime EndDate { get; set; }
    }
}
