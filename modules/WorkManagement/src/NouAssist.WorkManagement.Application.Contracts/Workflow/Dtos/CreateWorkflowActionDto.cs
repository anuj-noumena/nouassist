using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.WorkManagement.Workflow.Enums;

namespace NouAssist.WorkManagement.Workflow.Dtos
{
    public class CreateWorkflowActionDto
    {
        [Required]
        public Guid TransitionId { get; set; }
        
        [Required]
        [StringLength(256)]
        public string Name { get; set; }
        
        [StringLength(2000)]
        public string Description { get; set; }
        
        [Required]
        public WorkflowActionType ActionType { get; set; }
        
        public Dictionary<string, object> Configuration { get; set; }
        
        public int OrderIndex { get; set; }
        
        public bool IsAsync { get; set; }
        
        public bool ContinueOnError { get; set; }
    }
}