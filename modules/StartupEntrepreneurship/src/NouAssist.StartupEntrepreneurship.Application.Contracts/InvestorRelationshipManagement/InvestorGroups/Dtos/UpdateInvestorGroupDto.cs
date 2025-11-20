using System.ComponentModel.DataAnnotations;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.InvestorGroups.Dtos
{
    /// <summary>
    /// DTO for updating an existing investor group
    /// </summary>
    public class UpdateInvestorGroupDto
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }
    }
}
