using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.WorkManagement.Workflow.Dtos
{
    public class CreateWorkflowSchemeMappingDto
    {
        [Required]
        public Guid WorkflowSchemeId { get; set; }
        
        [Required]
        public Guid TaskTypeId { get; set; }
        
        [Required]
        public Guid WorkflowId { get; set; }
    }
}