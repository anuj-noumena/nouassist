using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Constants;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Enums;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.ResourceType.Dtos
{
    /// <summary>
    /// DTO for creating a new resource type
    /// </summary>
    public class CreateResourceTypeDto
    {
        /// <summary>
        /// Code uniquely identifying this resource type
        /// </summary>
        [Required]
        [StringLength(RIMConstants.ResourceTypes.MaxCodeLength)]
        public string Code { get; set; } = null!;

        /// <summary>
        /// Name of the resource type
        /// </summary>
        [Required]
        [StringLength(RIMConstants.ResourceTypes.MaxNameLength)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Description of the resource type
        /// </summary>
        [StringLength(RIMConstants.ResourceTypes.MaxDescriptionLength)]
        public string? Description { get; set; }

        /// <summary>
        /// Category of the resource type
        /// </summary>
        [Required]
        public ResourceCategory Category { get; set; }

        /// <summary>
        /// Path in the category hierarchy
        /// </summary>
        public string? CategoryPath { get; set; }

        /// <summary>
        /// Icon to represent this resource type
        /// </summary>
        [StringLength(100)]
        public string? Icon { get; set; }

        /// <summary>
        /// Color to represent this resource type
        /// </summary>
        [StringLength(20)]
        public string? Color { get; set; }
        
        /// <summary>
        /// List of attribute definitions for this resource type
        /// </summary>
        public List<CreateResourceAttributeDefinitionDto>? AttributeDefinitions { get; set; }
    }
}
