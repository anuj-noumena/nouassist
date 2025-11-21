using System.ComponentModel.DataAnnotations;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Constants;

namespace NouAssist.Analytics.DTOs.KPI
{
    /// <summary>
    /// DTO for updating an existing KPI definition
    /// </summary>
    public class UpdateKpiDefinitionDto
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
