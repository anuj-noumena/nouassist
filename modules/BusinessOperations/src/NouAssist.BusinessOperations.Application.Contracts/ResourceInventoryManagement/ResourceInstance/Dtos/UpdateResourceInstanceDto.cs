using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Constants;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.ResourceInstance.Dtos
{
    /// <summary>
    /// DTO for updating a resource instance
    /// </summary>
    public class UpdateResourceInstanceDto
    {
        /// <summary>
        /// Name of the resource instance
        /// </summary>
        [Required]
        [StringLength(RIMConstants.ResourceInstances.MaxNameLength)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Description of the resource instance
        /// </summary>
        [StringLength(RIMConstants.ResourceInstances.MaxDescriptionLength)]
        public string? Description { get; set; }

        /// <summary>
        /// User ID associated with this resource (for human resources)
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// Date when the resource is planned to be retired
        /// </summary>
        public DateTime? PlannedRetirementDate { get; set; }

        /// <summary>
        /// Additional tags for categorization and searching
        /// </summary>
        [StringLength(500)]
        public string? Tags { get; set; }

        /// <summary>
        /// Attribute values to update
        /// </summary>
        public List<CreateResourceAttributeValueDto>? AttributeValues { get; set; }
    }
}
