using System;
using System.Collections.Generic;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.ResourceInstance.Dtos
{
    /// <summary>
    /// DTO for resource instance information
    /// </summary>
    public class ResourceInstanceDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// ID of the resource type this instance belongs to
        /// </summary>
        public Guid ResourceTypeId { get; set; }
        
        /// <summary>
        /// Name of the resource type
        /// </summary>
        public string ResourceTypeName { get; set; } = null!;

        /// <summary>
        /// Unique identifier for this resource instance
        /// </summary>
        public string Identifier { get; set; } = null!;

        /// <summary>
        /// Name of the resource instance
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Description of the resource instance
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Current status of the resource instance
        /// </summary>
        public ResourceStatus Status { get; set; }

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
        /// Date when the resource was actually retired (if applicable)
        /// </summary>
        public DateTime? ActualRetirementDate { get; set; }

        /// <summary>
        /// Additional tags for categorization and searching
        /// </summary>
        public string? Tags { get; set; }

        /// <summary>
        /// Attribute values for this resource instance
        /// </summary>
        public List<ResourceAttributeValueDto> AttributeValues { get; set; } = new();
        
        /// <summary>
        /// Current allocation information (if allocated)
        /// </summary>
        public AllocationRecordDto? CurrentAllocation { get; set; }
    }
}
