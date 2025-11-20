using System;
using System.Collections.Generic;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.ResourceType.Dtos
{
    /// <summary>
    /// DTO for resource type information
    /// </summary>
    public class ResourceTypeDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// Code uniquely identifying this resource type
        /// </summary>
        public string Code { get; set; } = null!;

        /// <summary>
        /// Name of the resource type
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Description of the resource type
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Category of the resource type
        /// </summary>
        public ResourceCategory Category { get; set; }

        /// <summary>
        /// Path in the category hierarchy
        /// </summary>
        public string CategoryPath { get; set; } = null!;

        /// <summary>
        /// Indicates if this resource type is active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Icon to represent this resource type
        /// </summary>
        public string? Icon { get; set; }

        /// <summary>
        /// Color to represent this resource type
        /// </summary>
        public string? Color { get; set; }
        
        /// <summary>
        /// List of attribute definitions for this resource type
        /// </summary>
        public List<ResourceAttributeDefinitionDto> AttributeDefinitions { get; set; } = new();
    }
}
