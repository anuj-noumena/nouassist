using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.WorkflowOrchestration.WorkflowEngine.Workflows.Dtos
{
    public class CreateWorkflowSchemeDto
    {
        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        [Required]
        public Guid ProjectId { get; set; }

        public Guid? DefaultWorkflowId { get; set; }
    }
}
