using System;
using NouAssist.RequirementsManagement.Specification.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.Specification.Requirements.Dtos
{
    public class AcceptanceCriterionDto : EntityDto<Guid>
    {
        public string Statement { get; set; }
        public string Description { get; set; }
        public CriterionType Type { get; set; }
        public int Order { get; set; }
    }
}
