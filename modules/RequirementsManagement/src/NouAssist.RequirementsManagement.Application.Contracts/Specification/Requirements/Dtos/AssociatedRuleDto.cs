using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.Specification.Requirements.Dtos
{
    public class AssociatedRuleDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
