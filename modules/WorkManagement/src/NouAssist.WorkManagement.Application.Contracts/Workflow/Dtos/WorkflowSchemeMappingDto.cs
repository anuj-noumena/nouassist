using System;

namespace NouAssist.WorkManagement.Workflow.Dtos
{
    public class WorkflowSchemeMappingDto
    {
        public Guid Id { get; set; }
        public Guid WorkflowSchemeId { get; set; }
        public Guid TaskTypeId { get; set; }
        public Guid WorkflowId { get; set; }
        public bool IsActive { get; set; }
    }
}
