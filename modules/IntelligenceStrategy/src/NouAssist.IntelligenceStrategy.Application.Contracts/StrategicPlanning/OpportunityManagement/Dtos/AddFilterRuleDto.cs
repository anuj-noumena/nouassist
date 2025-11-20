using System.ComponentModel.DataAnnotations;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.OpportunityManagement.Dtos
{
    /// <summary>
    /// DTO for adding a filter rule
    /// </summary>
    public class AddFilterRuleDto
    {
        [Required]
        [StringLength(100)]
        public string RuleName { get; set; }

        [Required]
        [StringLength(100)]
        public string CriterionName { get; set; }

        [Required]
        [StringLength(500)]
        public string CriterionDescription { get; set; }

        [Required]
        public decimal MinThreshold { get; set; }

        [Required]
        public decimal MaxThreshold { get; set; }

        [Required]
        [Range(1, 10)]
        public int Weight { get; set; }

        [Required]
        [Range(1, 100)]
        public int Priority { get; set; }
    }
}
