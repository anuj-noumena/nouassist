using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.Specification.Constants;

namespace NouAssist.RequirementsManagement.Specification.SpecificationStructures.Dtos
{
    /// <summary>
    /// DTO for creating a new specification structure
    /// </summary>
    public class CreateSpecificationStructureDto
    {
        [Required]
        [StringLength(RequirementSpecificationManagementConsts.SpecificationStructure.MaxSectionNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string DocumentType { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }
    }
}
