using System;
using NouAssist.RequirementsManagement.Specification.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.Specification.Requirements.Dtos
{
    public class InterfaceSpecificationDto : EntityDto<Guid>
    {
        public string InterfaceName { get; set; }
        public string Endpoint { get; set; }
        public string HttpMethod { get; set; }
        public InterfaceType Type { get; set; }
        public string Description { get; set; }
    }
}
