using System.ComponentModel.DataAnnotations;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.OpportunityManagement.Dtos
{
    /// <summary>
    /// DTO for creating a new Opportunity Management Record
    /// </summary>
    public class CreateOpportunityManagementRecordDto
    {
        [Required]
        [StringLength(200)]
        public string RecordName { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        public FocusLevel InitialFocusLevel { get; set; }
    }
}
