using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.Specification.Constants;
using NouAssist.RequirementsManagement.Specification.Enums;

namespace NouAssist.RequirementsManagement.Specification.Requirements.Dtos
{
    public class CreateAssociatedConstraintDto
    {
        [Required]
        [StringLength(RequirementSpecificationManagementConsts.Constraint.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(RequirementSpecificationManagementConsts.Constraint.MaxStatementLength)]
        public string Statement { get; set; }

        [Required]
        public ConstraintType Type { get; set; }
    }
}
