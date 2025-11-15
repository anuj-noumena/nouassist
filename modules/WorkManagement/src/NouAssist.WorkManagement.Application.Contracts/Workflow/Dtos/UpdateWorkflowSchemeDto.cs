using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.WorkManagement.Workflow.Dtos
{
    public class UpdateWorkflowSchemeDto
    {
        [Required]
        [StringLength(256)]
        public string Name { get; set; }
        
        [StringLength(2000)]
        public string Description { get; set; }
        
        public Guid? DefaultWorkflowId { get; set; }
        
        public bool AllowQuickDelegation { get; set; }
        
        public bool RequireApprovalForIntegrated { get; set; }
    }
}