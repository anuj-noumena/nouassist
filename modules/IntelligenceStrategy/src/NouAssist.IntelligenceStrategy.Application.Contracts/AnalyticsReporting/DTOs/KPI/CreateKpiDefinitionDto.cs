using System.ComponentModel.DataAnnotations;
using NouAssist.Analytics.Constants;

namespace NouAssist.Analytics.DTOs.KPI
{
    /// <summary>
    /// DTO for creating a new KPI definition
    /// </summary>
    public class CreateKpiDefinitionDto
    {
        [Required]
        [StringLength(AnalyticsConsts.Kpi.MaxNameLength)]
        public string Name { get; set; }
        
        [StringLength(AnalyticsConsts.Kpi.MaxDescriptionLength)]
        public string Description { get; set; }
        
        [Required]
        [StringLength(1000)]
        public string CalculationExpression { get; set; }
    }
}
