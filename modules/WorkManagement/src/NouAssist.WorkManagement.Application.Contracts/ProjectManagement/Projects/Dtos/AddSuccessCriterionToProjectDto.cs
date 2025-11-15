using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.WorkManagement.ProjectManagement.Enums;

namespace NouAssist.WorkManagement.ProjectManagement.Projects.Dtos
{
    public class AddSuccessCriterionToProjectDto
    {
        [Required]
        [StringLength(500)]
        public string Title { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        [Required]
        public SuccessCriterionType CriterionType { get; set; }

        [StringLength(500)]
        public string MeasurementMethod { get; set; }

        [StringLength(500)]
        public string TargetValue { get; set; }

        [Range(0, 10)]
        public double Weight { get; set; } = 1.0;

        public DateTime? TargetDate { get; set; }
    }
}