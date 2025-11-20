using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.Specification.Requirements.Dtos
{
    public class BusinessObjectiveLinkDto : EntityDto<Guid>
    {
        public Guid ObjectiveId { get; set; }
        public string Justification { get; set; }
    }
}
