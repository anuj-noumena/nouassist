using System.ComponentModel.DataAnnotations;

namespace NouAssist.Analytics.DTOs.DataSource
{
    /// <summary>
    /// DTO for adding a transformation step to a data source
    /// </summary>
    public class AddTransformationStepDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        [Required]
        [StringLength(100)]
        public string TransformationType { get; set; }
        
        [Required]
        public string Configuration { get; set; }
        
        public int Order { get; set; }
    }
}
