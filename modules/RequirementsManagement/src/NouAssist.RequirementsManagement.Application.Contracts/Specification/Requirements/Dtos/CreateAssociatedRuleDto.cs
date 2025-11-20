using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.Specification.Constants;

namespace NouAssist.RequirementsManagement.Specification.Requirements.Dtos
{
    public class CreateAssociatedRuleDto
    {
        [Required]
        [StringLength(RequirementSpecificationManagementConsts.BusinessRule.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(RequirementSpecificationManagementConsts.BusinessRule.MaxDescriptionLength)]
        public string Description { get; set; }
    }
}
