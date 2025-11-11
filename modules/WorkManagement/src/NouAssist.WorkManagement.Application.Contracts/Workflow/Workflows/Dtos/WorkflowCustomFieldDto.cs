using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.WorkManagement.Workflow.Workflows.Dtos
{
    public class WorkflowCustomFieldDto : FullAuditedEntityDto<Guid>
    {
        public Guid WorkflowSchemeId { get; set; }
        public Guid CustomFieldDefinitionId { get; set; }
        public bool IsRequired { get; set; }
        public string DefaultValue { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public List<Guid> VisibleInStatusIds { get; set; } = new();
        public List<Guid> RequiredInStatusIds { get; set; } = new();
        public List<Guid> ReadOnlyInStatusIds { get; set; } = new();
        public List<Guid> RequiredForTransitionIds { get; set; } = new();
    }
}
