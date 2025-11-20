using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.SustainableGrowth.Dtos
{
    /// <summary>
    /// DTO for recording an ESG measurement
    /// </summary>
    public class RecordEsgMeasurementDto
    {
        [Required]
        [Range(0, double.MaxValue)]
        public decimal Value { get; set; }

        [Required]
        public DateTime MeasurementDate { get; set; }
    }
}
