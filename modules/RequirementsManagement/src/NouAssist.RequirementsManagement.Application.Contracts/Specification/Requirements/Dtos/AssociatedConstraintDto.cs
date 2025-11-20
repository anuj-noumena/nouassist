using System;
using NouAssist.RequirementsManagement.Specification.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.Specification.Requirements.Dtos
{
    public class AssociatedConstraintDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public string Statement { get; set; }
        public ConstraintType Type { get; set; }
    }
}
