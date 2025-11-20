using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.OpportunityManagement.Dtos
{
    /// <summary>
    /// DTO for evaluating an opportunity
    /// </summary>
    public class EvaluateOpportunityDto
    {
        [Required]
        public Dictionary<string, decimal> OpportunityMetrics { get; set; }

        public EvaluateOpportunityDto()
        {
            OpportunityMetrics = new Dictionary<string, decimal>();
        }
    }
}
