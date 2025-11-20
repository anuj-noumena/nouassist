using System.ComponentModel.DataAnnotations;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.InvestorProfiles.Dtos
{
    /// <summary>
    /// DTO for updating investor relationship score
    /// </summary>
    public class UpdateRelationshipScoreDto
    {
        [Required]
        [Range(0, 100)]
        public int Score { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }
    }
}
