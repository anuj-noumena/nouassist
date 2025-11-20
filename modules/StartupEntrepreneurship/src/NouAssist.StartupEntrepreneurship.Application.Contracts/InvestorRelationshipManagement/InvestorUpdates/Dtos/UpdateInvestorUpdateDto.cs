using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Enums;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.InvestorUpdates.Dtos
{
    /// <summary>
    /// DTO for updating an investor update (only allowed in draft status)
    /// </summary>
    public class UpdateInvestorUpdateDto
    {
        [Required]
        [StringLength(300)]
        public string Title { get; set; }

        [Required]
        [StringLength(500)]
        public string Subject { get; set; }

        public List<UpdateUpdateSectionDto> Sections { get; set; } = new();
        public List<CreateMetricSnapshotDto> Metrics { get; set; } = new();
    }
}
