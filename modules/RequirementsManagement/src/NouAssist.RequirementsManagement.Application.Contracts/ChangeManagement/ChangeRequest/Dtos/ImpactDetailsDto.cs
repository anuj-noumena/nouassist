using NouAssist.RequirementsManagement.ChangeManagement.Enums;

namespace NouAssist.RequirementsManagement.ChangeManagement.ChangeRequest.Dtos
{
    /// <summary>
    /// DTO for impact details
    /// </summary>
    public class ImpactDetailsDto
    {
        public ImpactType Type { get; set; }
        public string TypeString => Type.ToString();
        public decimal? CostImpact { get; set; }
        public int? TimeImpactDays { get; set; }
        public string Dependencies { get; set; }
    }
}
