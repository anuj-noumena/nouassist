using System.ComponentModel.DataAnnotations;

namespace NouAssist.WorkManagement.Workflow.Dtos
{
    public class UpdateWorkflowCustomFieldDto
    {
        public bool IsRequired { get; set; }
        
        [StringLength(500)]
        public string DefaultValue { get; set; }
        
        public int DisplayOrder { get; set; }
        
        public bool IsActive { get; set; }
        
        public FieldVisibilityRulesDto VisibilityRules { get; set; }
        public FieldValidationRulesDto ValidationRules { get; set; }
    }
}