using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.StrategicPivot.Dtos
{
    public class RecordMeasurementDto
    {
        [Required]
        public decimal Value { get; set; }

        [Required]
        public DateTime MeasuredAt { get; set; }

        [StringLength(500)]
        public string MeasurementMethod { get; set; }

        [StringLength(1000)]
        public string Notes { get; set; }
    }
}
