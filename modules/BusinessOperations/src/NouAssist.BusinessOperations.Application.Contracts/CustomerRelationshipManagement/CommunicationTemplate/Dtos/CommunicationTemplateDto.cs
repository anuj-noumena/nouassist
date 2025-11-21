using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Constants;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.CommunicationTemplate.Dtos
{
    /// <summary>
    /// Data transfer object for CommunicationTemplate entity
    /// </summary>
    public class CommunicationTemplateDto : EntityDto<Guid>
    {
        /// <summary>
        /// Unique code for the template
        /// </summary>
        public string Code { get; set; }
        
        /// <summary>
        /// Name of the template
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Type of communication this template is for
        /// </summary>
        public InteractionType Type { get; set; }
        
        /// <summary>
        /// Subject line for the template
        /// </summary>
        public string Subject { get; set; }
        
        /// <summary>
        /// Content of the template
        /// </summary>
        public string Content { get; set; }
        
        /// <summary>
        /// Whether this template is active
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// Optional category for organization
        /// </summary>
        public string Category { get; set; }
        
        /// <summary>
        /// Description of the template's purpose
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Creation time
        /// </summary>
        public DateTime CreationTime { get; set; }
        
        /// <summary>
        /// Last modification time
        /// </summary>
        public DateTime? LastModificationTime { get; set; }
    }

    /// <summary>
    /// Data transfer object for creating a communication template
    /// </summary>
    public class CreateCommunicationTemplateDto
    {
        /// <summary>
        /// Unique code for the template
        /// </summary>
        [Required]
        [StringLength(CRMConsts.MaxLengths.Code)]
        public string Code { get; set; }
        
        /// <summary>
        /// Name of the template
        /// </summary>
        [Required]
        [StringLength(CRMConsts.MaxLengths.Name)]
        public string Name { get; set; }
        
        /// <summary>
        /// Type of communication this template is for
        /// </summary>
        [Required]
        public InteractionType Type { get; set; }
        
        /// <summary>
        /// Subject line for the template
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Subject { get; set; }
        
        /// <summary>
        /// Content of the template
        /// </summary>
        [Required]
        [StringLength(4000)]
        public string Content { get; set; }
        
        /// <summary>
        /// Description of the template's purpose
        /// </summary>
        [StringLength(500)]
        public string Description { get; set; }
        
        /// <summary>
        /// Optional category for organization
        /// </summary>
        [StringLength(100)]
        public string Category { get; set; }
    }

    /// <summary>
    /// Data transfer object for updating a communication template
    /// </summary>
    public class UpdateCommunicationTemplateDto
    {
        /// <summary>
        /// Name of the template
        /// </summary>
        [StringLength(CRMConsts.MaxLengths.Name)]
        public string Name { get; set; }
        
        /// <summary>
        /// Subject line for the template
        /// </summary>
        [StringLength(200)]
        public string Subject { get; set; }
        
        /// <summary>
        /// Content of the template
        /// </summary>
        [StringLength(4000)]
        public string Content { get; set; }
        
        /// <summary>
        /// Description of the template's purpose
        /// </summary>
        [StringLength(500)]
        public string Description { get; set; }
        
        /// <summary>
        /// Optional category for organization
        /// </summary>
        [StringLength(100)]
        public string Category { get; set; }
    }

    /// <summary>
    /// Data transfer object for template list filtering and pagination
    /// </summary>
    public class GetCommunicationTemplateListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// Text to search in template name, code, and content
        /// </summary>
        public string Filter { get; set; }
        
        /// <summary>
        /// Filter by template type
        /// </summary>
        public InteractionType? Type { get; set; }
        
        /// <summary>
        /// Filter by category
        /// </summary>
        public string Category { get; set; }
        
        /// <summary>
        /// Filter by active status
        /// </summary>
        public bool? IsActive { get; set; }
    }

    /// <summary>
    /// Data transfer object for rendering a template
    /// </summary>
    public class RenderTemplateDto
    {
        /// <summary>
        /// Template variables for replacement
        /// </summary>
        [Required]
        public Dictionary<string, string> Variables { get; set; }
    }

    /// <summary>
    /// Data transfer object for rendered template result
    /// </summary>
    public class RenderedTemplateDto
    {
        /// <summary>
        /// Rendered subject
        /// </summary>
        public string Subject { get; set; }
        
        /// <summary>
        /// Rendered content
        /// </summary>
        public string Content { get; set; }
    }
}
