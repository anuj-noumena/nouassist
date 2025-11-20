using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Constants;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Enums;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.ResourceInstance.Dtos
{
    /// <summary>
    /// DTO for creating a new resource instance
    /// </summary>
    public class CreateResourceInstanceDto
    {
        /// <summary>
        /// ID of the resource type this instance belongs to
        /// </summary>
        [Required]
        public Guid ResourceTypeId { get; set; }

        /// <summary>
        /// Unique identifier for this resource instance
        /// </summary>
        [Required]
        [StringLength(RIMConstants.ResourceInstances.MaxIdentifierLength)]
        public string Identifier { get; set; } = null!;

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
        /// Initial status of the resource instance
        /// </summary>
        [Required]
        public ResourceStatus Status { get; set; } = ResourceStatus.Available;

        /// <summary>
        /// User ID associated with this resource (for human resources)
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// Date when the resource was acquired or hired
        /// </summary>
        public DateTime? AcquisitionDate { get; set; }

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
        /// Attribute values for this resource instance
        /// </summary>
        public List<CreateResourceAttributeValueDto>? AttributeValues { get; set; }
    }
}
