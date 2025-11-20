using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.Specification.Constants;

namespace NouAssist.RequirementsManagement.Specification.SpecificationStructures.Dtos
{
    public class UpdateSpecificationSectionDto
    {
        [Required]
        [StringLength(RequirementSpecificationManagementConsts.SpecificationStructure.MaxSectionNameLength)]
        public string SectionName { get; set; }

        [StringLength(RequirementSpecificationManagementConsts.SpecificationStructure.MaxSectionContentLength)]
        public string Content { get; set; }

        [Required]
        public int Order { get; set; }
    }
}
