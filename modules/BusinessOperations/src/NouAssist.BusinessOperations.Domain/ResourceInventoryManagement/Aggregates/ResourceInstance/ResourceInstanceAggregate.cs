using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Enums;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Events.ResourceInstance;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Exceptions;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Exceptions.ResourceInstance;
using NouAssist.BusinessOperations.ResourceInventoryManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.ResourceInstance
{
    /// <summary>
    /// Represents a specific resource instance in the system.
    /// Resource instances are concrete resources that can be allocated and tracked.
    /// </summary>
    public class ResourceInstanceAggregate : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// The tenant ID, if this resource instance belongs to a specific tenant
        /// </summary>
        public Guid? TenantId { get; protected set; }

        /// <summary>
        /// ID of the resource type this instance belongs to
        /// </summary>
        public Guid ResourceTypeId { get; private set; }

        /// <summary>
        /// Unique identifier for this resource instance
        /// </summary>
        public string Identifier { get; private set; }

        /// <summary>
        /// Name of the resource instance
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Description of the resource instance
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Current status of the resource instance
        /// </summary>
        public ResourceStatus Status { get; private set; }

        /// <summary>
        /// User ID associated with this resource (for human resources)
        /// </summary>
        public UserId UserId { get; private set; }

        /// <summary>
        /// Date when the resource was acquired or hired
        /// </summary>
        public DateTime? AcquisitionDate { get; private set; }

        /// <summary>
        /// Date when the resource is planned to be retired
        /// </summary>
        public DateTime? PlannedRetirementDate { get; private set; }

        /// <summary>
        /// Date when the resource was actually retired (if applicable)
        /// </summary>
        public DateTime? ActualRetirementDate { get; private set; }

        /// <summary>
        /// Additional tags for categorization and searching
        /// </summary>
        public string Tags { get; private set; }

        /// <summary>
        /// List of attribute values for this resource instance
        /// </summary>
        private readonly List<AttributeValue> _attributeValues;
        public IReadOnlyCollection<AttributeValue> AttributeValues => _attributeValues.AsReadOnly();

        /// <summary>
        /// List of allocation records for this resource instance
        /// </summary>
        private readonly List<AllocationRecord> _allocationRecords;
        public IReadOnlyCollection<AllocationRecord> AllocationRecords => _allocationRecords.AsReadOnly();

        /// <summary>
        /// List of usage records for this resource instance
        /// </summary>
        private readonly List<UsageRecord> _usageRecords;
        public IReadOnlyCollection<UsageRecord> UsageRecords => _usageRecords.AsReadOnly();

        /// <summary>
        /// Current allocation record (if allocated)
        /// </summary>
        public AllocationRecord CurrentAllocation => _allocationRecords
            .Where(a => a.EndTime == null || a.EndTime > DateTime.Now)
            .OrderByDescending(a => a.StartTime)
            .FirstOrDefault();

        protected ResourceInstanceAggregate()
        {
            _attributeValues = new List<AttributeValue>();
            _allocationRecords = new List<AllocationRecord>();
            _usageRecords = new List<UsageRecord>();
        }

        /// <summary>
        /// Creates a new resource instance
        /// </summary>
        /// <param name="id">Resource instance ID</param>
        /// <param name="resourceTypeId">Resource type ID</param>
        /// <param name="identifier">Unique identifier</param>
        /// <param name="name">Name</param>
        /// <param name="description">Description</param>
        /// <param name="status">Initial status</param>
        /// <param name="userId">Associated user ID (for human resources)</param>
        /// <param name="acquisitionDate">Acquisition date</param>
        /// <param name="tags">Tags for categorization</param>
        /// <param name="tenantId">Tenant ID</param>
        public ResourceInstanceAggregate(
            Guid id,
            Guid resourceTypeId,
            string identifier,
            string name,
            string description,
            ResourceStatus status,
            Guid? userId = null,
            DateTime? acquisitionDate = null,
            string tags = null,
            Guid? tenantId = null) : base(id)
        {
            ResourceTypeId = resourceTypeId;
            SetIdentifier(identifier);
            SetName(name);
            SetDescription(description);
            Status = status;
            UserId = userId.HasValue ? new UserId(userId.Value) : null;
            AcquisitionDate = acquisitionDate;
            Tags = tags;
            TenantId = tenantId;
            _attributeValues = new List<AttributeValue>();
            _allocationRecords = new List<AllocationRecord>();
            _usageRecords = new List<UsageRecord>();

            AddDistributedEvent(new ResourceInstanceCreatedEto
            {
                Id = Id,
                ResourceTypeId = ResourceTypeId,
                Name = Name,
                Identifier = Identifier,
                Status = Status.ToString(),
                UserId = userId,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Updates the resource instance information
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="description">Description</param>
        /// <param name="tags">Tags</param>
        public void Update(
            string name,
            string description,
            string tags = null)
        {
            SetName(name);
            SetDescription(description);
            Tags = tags;

            AddDistributedEvent(new ResourceInstanceUpdatedEto
            {
                Id = Id,
                ResourceTypeId = ResourceTypeId,
                Name = Name,
                Identifier = Identifier,
                Status = Status.ToString(),
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Sets or updates the user ID associated with this resource (for human resources)
        /// </summary>
        /// <param name="userId">User ID</param>
        public void SetUserId(Guid userId)
        {
            UserId = new UserId(userId);
        }

        /// <summary>
        /// Sets the attribute value for this resource instance
        /// </summary>
        /// <param name="attributeName">Attribute name</param>
        /// <param name="dataType">Attribute data type</param>
        /// <param name="value">Attribute value</param>
        public void SetAttributeValue(string attributeName, AttributeDataType dataType, object value)
        {
            var existingAttribute = _attributeValues.FirstOrDefault(a => a.Name == attributeName);
            if (existingAttribute != null)
            {
                _attributeValues.Remove(existingAttribute);
            }

            var attributeValue = new AttributeValue(attributeName, dataType, value);
            _attributeValues.Add(attributeValue);
        }

        /// <summary>
        /// Gets the attribute value for the specified attribute name
        /// </summary>
        /// <param name="attributeName">Attribute name</param>
        /// <returns>Attribute value, or null if not found</returns>
        public AttributeValue GetAttributeValue(string attributeName)
        {
            return _attributeValues.FirstOrDefault(a => a.Name == attributeName);
        }

        /// <summary>
        /// Marks this resource as available
        /// </summary>
        public void MarkAsAvailable()
        {
            // Cannot mark as available if currently allocated
            if (Status == ResourceStatus.Allocated && CurrentAllocation != null)
            {
                throw new ResourceAllocationException(Id, "Resource is currently allocated and cannot be marked as available");
            }

            Status = ResourceStatus.Available;
        }

        /// <summary>
        /// Marks this resource as unavailable
        /// </summary>
        /// <param name="reason">Reason for unavailability</param>
        public void MarkAsUnavailable(string reason)
        {
            // End any current allocation if resource is being marked as unavailable
            if (Status == ResourceStatus.Allocated && CurrentAllocation != null)
            {
                EndAllocation(DateTime.Now, reason);
            }

            Status = ResourceStatus.Unavailable;
        }

        /// <summary>
        /// Retires this resource
        /// </summary>
        /// <param name="retirementDate">Retirement date (defaults to now)</param>
        /// <param name="reason">Reason for retirement</param>
        public void Retire(DateTime? retirementDate = null, string reason = null)
        {
            // End any current allocation if resource is being retired
            if (Status == ResourceStatus.Allocated && CurrentAllocation != null)
            {
                EndAllocation(DateTime.Now, $"Resource retired: {reason}");
            }

            Status = ResourceStatus.Retired;
            ActualRetirementDate = retirementDate ?? DateTime.Now;
        }

        /// <summary>
        /// Allocates this resource
        /// </summary>
        /// <param name="allocationId">Allocation record ID</param>
        /// <param name="allocatedToEntity">Entity the resource is allocated to</param>
        /// <param name="allocatedToEntityId">ID of the entity the resource is allocated to</param>
        /// <param name="startTime">Allocation start time</param>
        /// <param name="endTime">Allocation end time (optional)</param>
        /// <param name="allocationPercentage">Allocation percentage (optional)</param>
        /// <param name="notes">Allocation notes (optional)</param>
        /// <returns>The new allocation record</returns>
        public AllocationRecord Allocate(
            Guid allocationId,
            string allocatedToEntity,
            Guid allocatedToEntityId,
            DateTime startTime,
            DateTime? endTime = null,
            decimal? allocationPercentage = null,
            string notes = null)
        {
            // Check if resource can be allocated
            if (Status == ResourceStatus.Retired)
            {
                throw new ResourceAllocationException(Id, "Cannot allocate a retired resource");
            }

            if (Status == ResourceStatus.Unavailable)
            {
                throw new ResourceAllocationException(Id, "Cannot allocate an unavailable resource");
            }

            // End any current allocation if already allocated
            if (Status == ResourceStatus.Allocated && CurrentAllocation != null)
            {
                EndAllocation(startTime, "New allocation started");
            }

            // Create new allocation record
            var allocationRecord = new AllocationRecord(
                allocationId,
                Id,
                allocatedToEntity,
                allocatedToEntityId,
                startTime,
                endTime,
                allocationPercentage,
                notes);

            _allocationRecords.Add(allocationRecord);
            Status = ResourceStatus.Allocated;

            AddDistributedEvent(new ResourceAllocatedEto
            {
                ResourceInstanceId = Id,
                AllocationId = allocationId,
                AllocatedToEntity = allocatedToEntity,
                AllocatedToEntityId = allocatedToEntityId,
                StartTime = startTime,
                EndTime = endTime,
                AllocationPercentage = allocationPercentage,
                TenantId = TenantId
            });

            return allocationRecord;
        }

        /// <summary>
        /// Ends the current allocation
        /// </summary>
        /// <param name="endTime">End time</param>
        /// <param name="notes">End allocation notes (optional)</param>
        /// <returns>The updated allocation record</returns>
        public AllocationRecord EndAllocation(DateTime endTime, string notes = null)
        {
            var currentAllocation = CurrentAllocation;
            if (currentAllocation == null)
            {
                throw new ResourceAllocationException(Id, "Resource is not currently allocated");
            }

            currentAllocation.End(endTime, notes);
            Status = ResourceStatus.Available;

            AddDistributedEvent(new ResourceAllocationEndedEto
            {
                ResourceInstanceId = Id,
                AllocationId = currentAllocation.Id,
                EndTime = endTime,
                TenantId = TenantId
            });

            return currentAllocation;
        }

        /// <summary>
        /// Logs usage of this resource
        /// </summary>
        /// <param name="usageId">Usage record ID</param>
        /// <param name="usageType">Type of usage</param>
        /// <param name="startTime">Usage start time</param>
        /// <param name="endTime">Usage end time</param>
        /// <param name="quantity">Usage quantity (optional)</param>
        /// <param name="unitOfMeasure">Unit of measure (optional)</param>
        /// <param name="relatedEntityType">Related entity type (optional)</param>
        /// <param name="relatedEntityId">Related entity ID (optional)</param>
        /// <param name="notes">Usage notes (optional)</param>
        /// <returns>The new usage record</returns>
        public UsageRecord LogUsage(
            Guid usageId,
            string usageType,
            DateTime startTime,
            DateTime endTime,
            decimal? quantity = null,
            string unitOfMeasure = null,
            string relatedEntityType = null,
            Guid? relatedEntityId = null,
            string notes = null)
        {
            // Create new usage record
            var usageRecord = new UsageRecord(
                usageId,
                Id,
                usageType,
                startTime,
                endTime,
                quantity,
                unitOfMeasure,
                relatedEntityType,
                relatedEntityId,
                notes);

            _usageRecords.Add(usageRecord);

            AddDistributedEvent(new ResourceUsageLoggedEto
            {
                ResourceInstanceId = Id,
                UsageId = usageId,
                UsageType = usageType,
                StartTime = startTime,
                EndTime = endTime,
                Quantity = quantity,
                UnitOfMeasure = unitOfMeasure,
                TenantId = TenantId
            });

            return usageRecord;
        }

        /// <summary>
        /// Sets the planned retirement date for this resource
        /// </summary>
        /// <param name="plannedRetirementDate">Planned retirement date</param>
        public void SetPlannedRetirementDate(DateTime plannedRetirementDate)
        {
            PlannedRetirementDate = plannedRetirementDate;
        }

        private void SetIdentifier(string identifier)
        {
            Identifier = Check.NotNullOrWhiteSpace(
                identifier,
                nameof(identifier),
                maxLength: Constants.RIMConstants.ResourceInstances.MaxIdentifierLength
            );
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: Constants.RIMConstants.ResourceInstances.MaxNameLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.Length(
                description,
                nameof(description),
                maxLength: Constants.RIMConstants.ResourceInstances.MaxDescriptionLength
            );
        }
    }
}
