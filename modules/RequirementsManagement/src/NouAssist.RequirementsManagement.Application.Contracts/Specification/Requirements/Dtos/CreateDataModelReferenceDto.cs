using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.Specification.Constants;
using NouAssist.RequirementsManagement.Specification.Enums;

namespace NouAssist.RequirementsManagement.Specification.Requirements.Dtos
{
    public class CreateDataModelReferenceDto
    {
        [Required]
        [StringLength(RequirementSpecificationManagementConsts.DataRequirement.MaxFieldNameLength)]
        public string FieldName { get; set; }

        [Required]
        public DataFieldType DataType { get; set; }

        public bool IsRequired { get; set; }

        [StringLength(RequirementSpecificationManagementConsts.DataRequirement.MaxDescriptionLength)]
        public string Description { get; set; }

        public int? MaxLength { get; set; }
    }
}
