using System.ComponentModel.DataAnnotations;

namespace NouAssist.Analytics.DTOs.Report
{
    /// <summary>
    /// DTO for adding a parameter to a report
    /// </summary>
    public class AddReportParameterDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        [Required]
        [StringLength(200)]
        public string DisplayName { get; set; }
        
        [Required]
        [StringLength(50)]
        public string DataType { get; set; }
        
        public bool IsRequired { get; set; }
        
        public string DefaultValue { get; set; }
        
        public int Order { get; set; }
        
        [StringLength(500)]
        public string ValidationRegex { get; set; }
    }
}
