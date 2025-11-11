using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.WorkManagement.Workflow.Workflows.Dtos
{
    public class WorkflowDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid InitialStatusId { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }

        public List<WorkflowStatusDto> Statuses { get; set; } = new();
        public List<WorkflowTransitionDto> Transitions { get; set; } = new();
    }
}
