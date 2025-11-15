using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.WorkManagement.Workflow.Dtos
{
    public class WorkflowCustomFieldDto : FullAuditedEntityDto<Guid>
    {
        public Guid WorkflowSchemeId { get; set; }
        public Guid CustomFieldDefinitionId { get; set; }
        public bool IsRequired { get; set; }
        public string DefaultValue { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        
        // Changed: Now expose the rules objects instead of simple lists
        public FieldVisibilityRulesDto VisibilityRules { get; set; }
        public FieldValidationRulesDto ValidationRules { get; set; }
    }
    
    /// <summary>
    /// DTO for field visibility rules
    /// </summary>
    public class FieldVisibilityRulesDto
    {
        public List<Guid> VisibleInStatusIds { get; set; } = new();
        public List<Guid> HiddenInStatusIds { get; set; } = new();
        public List<Guid> ReadOnlyInStatusIds { get; set; } = new();
    }
    
    /// <summary>
    /// DTO for field validation rules
    /// </summary>
    public class FieldValidationRulesDto
    {
        // String validations
        public int? MinLength { get; set; }
        public int? MaxLength { get; set; }
        public string RegexPattern { get; set; }
        public string RegexErrorMessage { get; set; }
        
        // Numeric validations
        public decimal? MinValue { get; set; }
        public decimal? MaxValue { get; set; }
        
        // Date validations
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        public bool MustBeFutureDate { get; set; }
        public bool MustBePastDate { get; set; }
        
        // Collection validations
        public int? MinSelections { get; set; }
        public int? MaxSelections { get; set; }
        public List<string> AllowedValues { get; set; } = new();
        public List<string> DisallowedValues { get; set; } = new();
        
        // Status-based validations
        public List<Guid> RequiredInStatusIds { get; set; } = new();
        public List<Guid> OptionalInStatusIds { get; set; } = new();
        
        // Custom validations
        public string CustomValidationExpression { get; set; }
        public string CustomErrorMessage { get; set; }
    }
}