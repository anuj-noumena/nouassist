using System;
using NouAssist.RequirementsManagement.Elicitation.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.Elicitation.StakeholderProfiles.Dtos
{
    /// <summary>
    /// DTO representing a stakeholder profile
    /// </summary>
    public class StakeholderProfileDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public StakeholderRole Role { get; set; }
        public string? Organization { get; set; }
        public ContactInfoDto ContactInfo { get; set; } = new();
        public InfluenceDataDto InfluenceData { get; set; } = new();
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// DTO for contact information
    /// </summary>
    public class ContactInfoDto
    {
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }

    /// <summary>
    /// DTO for influence and interest data
    /// </summary>
    public class InfluenceDataDto
    {
        public int InfluenceLevel { get; set; }
        public int InterestLevel { get; set; }
        public string? Notes { get; set; }
        public string InfluenceCategory { get; set; } = string.Empty;
        public string InterestCategory { get; set; } = string.Empty;
    }
}
