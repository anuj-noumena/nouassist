using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.Specification.Constants;
using NouAssist.RequirementsManagement.Specification.Enums;

namespace NouAssist.RequirementsManagement.Specification.Requirements.Dtos
{
    public class CreateAcceptanceCriterionDto
    {
        [Required]
        [StringLength(RequirementSpecificationManagementConsts.AcceptanceCriterion.MaxStatementLength)]
        public string Statement { get; set; }

        [StringLength(RequirementSpecificationManagementConsts.AcceptanceCriterion.MaxDescriptionLength)]
        public string Description { get; set; }

        [Required]
        public CriterionType Type { get; set; }
    }
}
