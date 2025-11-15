using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.WorkManagement.Workflow.Enums;

namespace NouAssist.WorkManagement.Workflow.Dtos
{
    public class CreateWorkflowTransitionDto
    {
        [Required]
        public Guid WorkflowId { get; set; }
        
        [Required]
        [StringLength(256)]
        public string Name { get; set; }
        
        [StringLength(2000)]
        public string Description { get; set; }
        
        [Required]
        public Guid FromStatusId { get; set; }
        
        [Required]
        public Guid ToStatusId { get; set; }
        
        public TransitionPurpose Purpose { get; set; } = TransitionPurpose.Standard;
        
        public bool RequiresComment { get; set; }
        public bool RequiresResolution { get; set; }
        public bool IsGlobal { get; set; }
        public int OrderIndex { get; set; }
        
        [StringLength(100)]
        public string ButtonLabel { get; set; }
        
        [StringLength(20)]
        public string ButtonColor { get; set; }
    }
}