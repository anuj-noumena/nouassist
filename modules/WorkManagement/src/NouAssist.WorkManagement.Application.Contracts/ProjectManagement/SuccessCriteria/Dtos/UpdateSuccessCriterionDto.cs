using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.WorkManagement.ProjectManagement.SuccessCriteria.Dtos
{
    public class UpdateSuccessCriterionDto
    {
        [Required]
        [StringLength(500)]
        public string Title { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        [StringLength(500)]
        public string MeasurementMethod { get; set; }

        [StringLength(500)]
        public string TargetValue { get; set; }

        public double? TargetNumeric { get; set; }

        [StringLength(50)]
        public string Unit { get; set; }

        [Range(0, 10)]
        public double Weight { get; set; }

        public DateTime? TargetDate { get; set; }

        [StringLength(5000)]
        public string Notes { get; set; }
    }
}
