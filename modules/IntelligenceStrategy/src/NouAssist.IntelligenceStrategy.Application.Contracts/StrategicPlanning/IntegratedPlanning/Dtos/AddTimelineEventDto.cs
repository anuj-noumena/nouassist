using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.IntegratedPlanning.Dtos
{
    /// <summary>
    /// DTO for adding a timeline event
    /// </summary>
    public class AddTimelineEventDto
    {
        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        [StringLength(50)]
        public string EventType { get; set; }

        public bool IsCriticalPath { get; set; }

        [StringLength(1000)]
        public string Dependencies { get; set; }
    }
}
