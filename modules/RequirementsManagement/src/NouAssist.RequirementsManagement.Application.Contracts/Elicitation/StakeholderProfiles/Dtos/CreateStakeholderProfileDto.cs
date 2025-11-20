using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.Elicitation.Constants;
using NouAssist.RequirementsManagement.Elicitation.Enums;

namespace NouAssist.RequirementsManagement.Elicitation.StakeholderProfiles.Dtos
{
    /// <summary>
    /// DTO for creating a new stakeholder profile
    /// </summary>
    public class CreateStakeholderProfileDto
    {
        [Required]
        [StringLength(ElicitationConsts.MaxStakeholderNameLength)]
        public string Name { get; set; } = string.Empty;

        [StringLength(ElicitationConsts.MaxStakeholderDescriptionLength)]
        public string? Description { get; set; }

        [Required]
        public StakeholderRole Role { get; set; }

        [StringLength(ElicitationConsts.MaxOrganizationLength)]
        public string? Organization { get; set; }

        [Required]
        public CreateContactInfoDto ContactInfo { get; set; } = new();

        [Required]
        public CreateInfluenceDataDto InfluenceData { get; set; } = new();
    }

    /// <summary>
    /// DTO for creating contact information
    /// </summary>
    public class CreateContactInfoDto
    {
        [Required]
        [EmailAddress]
        [StringLength(ElicitationConsts.MaxEmailLength)]
        public string Email { get; set; } = string.Empty;

        [Phone]
        [StringLength(ElicitationConsts.MaxPhoneLength)]
        public string? Phone { get; set; }

        [StringLength(ElicitationConsts.MaxAddressLength)]
        public string? Address { get; set; }
    }

    /// <summary>
    /// DTO for creating influence data
    /// </summary>
    public class CreateInfluenceDataDto
    {
        [Range(ElicitationConsts.MinInfluenceLevel, ElicitationConsts.MaxInfluenceLevel)]
        public int InfluenceLevel { get; set; } = 5;

        [Range(ElicitationConsts.MinInterestLevel, ElicitationConsts.MaxInterestLevel)]
        public int InterestLevel { get; set; } = 5;

        [StringLength(500)]
        public string? Notes { get; set; }
    }
}
