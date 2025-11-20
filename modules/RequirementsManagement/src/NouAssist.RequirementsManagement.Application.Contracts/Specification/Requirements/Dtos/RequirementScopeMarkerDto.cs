using System;
using NouAssist.RequirementsManagement.Specification.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.Specification.Requirements.Dtos
{
    public class RequirementScopeMarkerDto : EntityDto<Guid>
    {
        public ScopeStatus ScopeStatus { get; set; }
        public string Justification { get; set; }
        public DateTime MarkedAt { get; set; }
        public Guid MarkedBy { get; set; }
        public string MarkedByUserName { get; set; }
    }
}
