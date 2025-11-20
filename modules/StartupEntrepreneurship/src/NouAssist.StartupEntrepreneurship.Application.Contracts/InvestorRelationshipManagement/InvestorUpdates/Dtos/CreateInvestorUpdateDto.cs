using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Enums;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.InvestorUpdates.Dtos
{
    /// <summary>
    /// DTO for creating a new investor update
    /// </summary>
    public class CreateInvestorUpdateDto
    {
        [Required]
        [StringLength(300)]
        public string Title { get; set; }

        [Required]
        [StringLength(500)]
        public string Subject { get; set; }

        [Required]
        public UpdateFrequency Frequency { get; set; }

        [StringLength(100)]
        public string TemplateId { get; set; }

        public List<CreateUpdateSectionDto> Sections { get; set; } = new();
        public List<CreateMetricSnapshotDto> Metrics { get; set; } = new();
    }
}
