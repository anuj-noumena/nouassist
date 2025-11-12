using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.WorkManagement.Workflow.Dtos
{
    public class WorkflowSchemeDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid ProjectId { get; set; }
        public Guid? DefaultWorkflowId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDraft { get; set; }

        public List<WorkflowSchemeMappingDto> Mappings { get; set; } = new();
    }
}
