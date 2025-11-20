using System.ComponentModel.DataAnnotations;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.CrisisManagement.Dtos
{
    /// <summary>
    /// DTO for activating a crisis management plan
    /// </summary>
    public class ActivateCrisisPlanDto
    {
        [StringLength(1000)]
        public string Reason { get; set; }
    }
}
