using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.WorkManagement.Workflow.Enums;

namespace NouAssist.WorkManagement.Workflow.Dtos
{
    public class CreateWorkflowStatusDto
    {
        [Required]
        public Guid WorkflowId { get; set; }
        
        [Required]
        [StringLength(256)]
        public string Name { get; set; }
        
        [StringLength(2000)]
        public string Description { get; set; }
        
        [Required]
        public WorkflowStatusCategory Category { get; set; }
        
        [StringLength(20)]
        public string Color { get; set; }
        
        public int OrderIndex { get; set; }
        
        public bool IsInitial { get; set; }
        
        public bool IsResolution { get; set; }
    }
}