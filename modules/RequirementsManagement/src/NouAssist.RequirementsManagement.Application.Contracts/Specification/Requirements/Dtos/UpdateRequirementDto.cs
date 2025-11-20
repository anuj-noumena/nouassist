using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.Specification.Constants;
using NouAssist.RequirementsManagement.Specification.Enums;

namespace NouAssist.RequirementsManagement.Specification.Requirements.Dtos
{
    /// <summary>
    /// DTO for updating an existing requirement
    /// </summary>
    public class UpdateRequirementDto
    {
        [Required]
        [StringLength(RequirementSpecificationManagementConsts.Requirement.MaxTitleLength)]
        public string Title { get; set; }

        [StringLength(RequirementSpecificationManagementConsts.Requirement.MaxDescriptionLength)]
        public string Description { get; set; }

        [Required]
        public RequirementPriority Priority { get; set; }
    }
}
