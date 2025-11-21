using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Enums;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.GrowthExperiments.Dtos
{
    public class RecordExperimentResultDto
    {
        [Required]
        public Guid VariantId { get; set; }
        
        [Required]
        [StringLength(100)]
        public string MetricName { get; set; }
        
        public MetricType MetricType { get; set; }
        
        public double Value { get; set; }
        
        [Range(1, int.MaxValue)]
        public int SampleSize { get; set; }
        
        public double? StandardDeviation { get; set; }
        
        public double? MarginOfError { get; set; }
        
        public bool IsPrimary { get; set; }
    }
}
