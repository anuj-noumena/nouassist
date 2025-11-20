using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.Specification.Constants;
using NouAssist.RequirementsManagement.Specification.Enums;

namespace NouAssist.RequirementsManagement.Specification.Requirements.Dtos
{
    /// <summary>
    /// DTO for creating a new requirement
    /// </summary>
    public class CreateRequirementDto
    {
        [Required]
        [StringLength(RequirementSpecificationManagementConsts.Requirement.MaxIdentifierLength)]
        public string Identifier { get; set; }

        [Required]
        [StringLength(RequirementSpecificationManagementConsts.Requirement.MaxTitleLength)]
        public string Title { get; set; }

        [StringLength(RequirementSpecificationManagementConsts.Requirement.MaxDescriptionLength)]
        public string Description { get; set; }

        [Required]
        public RequirementType Type { get; set; }

        [Required]
        public RequirementPriority Priority { get; set; }
    }
}
