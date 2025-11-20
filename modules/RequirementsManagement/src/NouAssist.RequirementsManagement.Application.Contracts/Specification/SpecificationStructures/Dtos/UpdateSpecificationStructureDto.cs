using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.Specification.Constants;

namespace NouAssist.RequirementsManagement.Specification.SpecificationStructures.Dtos
{
    /// <summary>
    /// DTO for updating an existing specification structure
    /// </summary>
    public class UpdateSpecificationStructureDto
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
