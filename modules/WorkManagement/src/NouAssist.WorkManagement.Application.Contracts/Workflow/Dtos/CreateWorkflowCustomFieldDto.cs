using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.WorkManagement.Workflow.Dtos
{
    public class CreateWorkflowCustomFieldDto
    {
        [Required]
        public Guid WorkflowSchemeId { get; set; }
        
        [Required]
        public Guid CustomFieldDefinitionId { get; set; }
        
        public bool IsRequired { get; set; }
        
        [StringLength(500)]
        public string DefaultValue { get; set; }
        
        public int DisplayOrder { get; set; }
        
        public FieldVisibilityRulesDto VisibilityRules { get; set; }
        public FieldValidationRulesDto ValidationRules { get; set; }
    }
}