using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.StrategicGoals.Dtos
{
    /// <summary>
    /// DTO for updating target progress
    /// </summary>
    public class UpdateTargetProgressDto
    {
        [Required]
        [Range(0, double.MaxValue)]
        public decimal CurrentValue { get; set; }

        [Required]
        public DateTime MeasurementDate { get; set; }
    }
}
