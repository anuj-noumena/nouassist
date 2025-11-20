using System.ComponentModel.DataAnnotations;

namespace NouAssist.Analytics.DTOs.DataSource
{
    /// <summary>
    /// DTO for adding an aggregated data view to a data source
    /// </summary>
    public class AddAggregatedDataViewDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        [Required]
        public string ViewDefinition { get; set; }
        
        [Required]
        [StringLength(50)]
        public string RefreshStrategy { get; set; }
    }
}
