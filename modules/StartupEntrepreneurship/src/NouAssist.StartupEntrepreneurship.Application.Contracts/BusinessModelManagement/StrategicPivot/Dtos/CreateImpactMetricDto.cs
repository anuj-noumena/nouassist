using System.ComponentModel.DataAnnotations;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.StrategicPivot.Dtos
{
    public class CreateImpactMetricDto
    {
        [Required]
        [StringLength(100)]
        public string MetricName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [StringLength(50)]
        public string Unit { get; set; }

        [StringLength(500)]
        public string MeasurementMethod { get; set; }

        public decimal? BaselineValue { get; set; }
        public decimal? TargetValue { get; set; }
    }
}
