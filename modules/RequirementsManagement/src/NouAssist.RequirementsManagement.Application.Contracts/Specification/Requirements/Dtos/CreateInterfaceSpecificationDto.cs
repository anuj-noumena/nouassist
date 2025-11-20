using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.Specification.Constants;
using NouAssist.RequirementsManagement.Specification.Enums;

namespace NouAssist.RequirementsManagement.Specification.Requirements.Dtos
{
    public class CreateInterfaceSpecificationDto
    {
        [Required]
        [StringLength(RequirementSpecificationManagementConsts.ExternalInterface.MaxNameLength)]
        public string InterfaceName { get; set; }

        [Required]
        [StringLength(RequirementSpecificationManagementConsts.ExternalInterface.MaxEndpointLength)]
        public string Endpoint { get; set; }

        [Required]
        [StringLength(10)]
        public string HttpMethod { get; set; }

        [Required]
        public InterfaceType Type { get; set; }

        [StringLength(RequirementSpecificationManagementConsts.ExternalInterface.MaxDescriptionLength)]
        public string Description { get; set; }
    }
}
