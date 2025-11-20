using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Events.Location;
using NouAssist.BusinessOperations.ResourceInventoryManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.StorageLocation
{
    /// <summary>
    /// Represents a physical or logical storage location for inventory items.
    /// </summary>
    public class StorageLocationAggregate : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// The tenant ID, if this storage location belongs to a specific tenant
        /// </summary>
        public Guid? TenantId { get; protected set; }

        /// <summary>
        /// Code uniquely identifying this location
        /// </summary>
        public string Code { get; private set; }

        /// <summary>
        /// Name of the storage location
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Description of the storage location
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// ID of the parent location in the hierarchy
        /// </summary>
        public Guid? ParentLocationId { get; private set; }

        /// <summary>
        /// Type of location (warehouse, shelf, bin, etc.)
        /// </summary>
        public string LocationType { get; private set; }

        /// <summary>
        /// Physical address of the location (optional)
        /// </summary>
        public string Address { get; private set; }

        /// <summary>
        /// Geographic coordinates of the location (optional)
        /// </summary>
        public LocationCoordinates Coordinates { get; private set; }

        /// <summary>
        /// Maximum capacity of the location (optional)
        /// </summary>
        public decimal? MaxCapacity { get; private set; }

        /// <summary>
        /// Unit of measure for the capacity
        /// </summary>
        public string CapacityUnitOfMeasure { get; private set; }

        /// <summary>
        /// Indicates if this storage location is active
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// Additional tags for categorization and searching
        /// </summary>
        public string Tags { get; private set; }

        /// <summary>
        /// List of child locations in the hierarchy
        /// </summary>
        private readonly List<LocationHierarchyNode> _childLocations;
        public IReadOnlyCollection<LocationHierarchyNode> ChildLocations => _childLocations.AsReadOnly();

        protected StorageLocationAggregate()
        {
            _childLocations = new List<LocationHierarchyNode>();
        }

        /// <summary>
        /// Creates a new storage location
        /// </summary>
        /// <param name="id">Storage location ID</param>
        /// <param name="code">Unique code</param>
        /// <param name="name">Name</param>
        /// <param name="description">Description</param>
        /// <param name="locationType">Location type</param>
        /// <param name="parentLocationId">Parent location ID (optional)</param>
        /// <param name="address">Physical address (optional)</param>
        /// <param name="coordinates">Geographic coordinates (optional)</param>
        /// <param name="maxCapacity">Maximum capacity (optional)</param>
        /// <param name="capacityUnitOfMeasure">Unit of measure for capacity (optional)</param>
        /// <param name="tags">Tags (optional)</param>
        /// <param name="tenantId">Tenant ID (optional)</param>
        public StorageLocationAggregate(
            Guid id,
            string code,
            string name,
            string description,
            string locationType,
            Guid? parentLocationId = null,
            string address = null,
            LocationCoordinates coordinates = null,
            decimal? maxCapacity = null,
            string capacityUnitOfMeasure = null,
            string tags = null,
            Guid? tenantId = null) : base(id)
        {
            SetCode(code);
            SetName(name);
            SetDescription(description);
            LocationType = locationType;
            ParentLocationId = parentLocationId;
            Address = address;
            Coordinates = coordinates;
            MaxCapacity = maxCapacity;
            CapacityUnitOfMeasure = capacityUnitOfMeasure;
            Tags = tags;
            IsActive = true;
            TenantId = tenantId;
            _childLocations = new List<LocationHierarchyNode>();

            AddDistributedEvent(new StorageLocationCreatedEto
            {
                Id = Id,
                Code = Code,
                Name = Name,
                ParentLocationId = ParentLocationId,
                LocationType = LocationType,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Updates the storage location information
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="description">Description</param>
        /// <param name="locationType">Location type</param>
        /// <param name="address">Physical address (optional)</param>
        /// <param name="coordinates">Geographic coordinates (optional)</param>
        /// <param name="maxCapacity">Maximum capacity (optional)</param>
        /// <param name="capacityUnitOfMeasure">Unit of measure for capacity (optional)</param>
        /// <param name="tags">Tags (optional)</param>
        public void Update(
            string name,
            string description,
            string locationType,
            string address = null,
            LocationCoordinates coordinates = null,
            decimal? maxCapacity = null,
            string capacityUnitOfMeasure = null,
            string tags = null)
        {
            SetName(name);
            SetDescription(description);
            LocationType = locationType;
            Address = address;
            Coordinates = coordinates;
            MaxCapacity = maxCapacity;
            CapacityUnitOfMeasure = capacityUnitOfMeasure;
            Tags = tags;

            AddDistributedEvent(new StorageLocationUpdatedEto
            {
                Id = Id,
                Code = Code,
                Name = Name,
                ParentLocationId = ParentLocationId,
                LocationType = LocationType,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Changes the parent location
        /// </summary>
        /// <param name="newParentLocationId">New parent location ID (null for root location)</param>
        public void ChangeParent(Guid? newParentLocationId)
        {
            // Prevent circular references
            if (newParentLocationId == Id)
            {
                throw new RIMDomainException("RIM:ErrorMessage:CircularLocationReference");
            }

            ParentLocationId = newParentLocationId;

            AddDistributedEvent(new StorageLocationParentChangedEto
            {
                Id = Id,
                ParentLocationId = ParentLocationId,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Deactivates this storage location
        /// </summary>
        public void Deactivate()
        {
            if (!IsActive)
            {
                return;
            }

            IsActive = false;
        }

        /// <summary>
        /// Activates this storage location
        /// </summary>
        public void Activate()
        {
            if (IsActive)
            {
                return;
            }

            IsActive = true;
        }

        /// <summary>
        /// Adds a child location to this location
        /// </summary>
        /// <param name="childLocationId">Child location ID</param>
        /// <param name="childLocationCode">Child location code</param>
        /// <param name="childLocationName">Child location name</param>
        /// <returns>The created hierarchy node</returns>
        public LocationHierarchyNode AddChildLocation(
            Guid childLocationId,
            string childLocationCode,
            string childLocationName)
        {
            // Check if already a child
            if (_childLocations.Any(cl => cl.ChildLocationId == childLocationId))
            {
                throw new RIMDomainException("RIM:ErrorMessage:LocationAlreadyChild")
                    .WithData("childLocationId", childLocationId)
                    .WithData("parentLocationId", Id);
            }

            var hierarchyNode = new LocationHierarchyNode(
                Id,
                childLocationId,
                childLocationCode,
                childLocationName);

            _childLocations.Add(hierarchyNode);

            return hierarchyNode;
        }

        /// <summary>
        /// Removes a child location from this location
        /// </summary>
        /// <param name="childLocationId">Child location ID</param>
        public void RemoveChildLocation(Guid childLocationId)
        {
            var hierarchyNode = _childLocations.FirstOrDefault(cl => cl.ChildLocationId == childLocationId);
            if (hierarchyNode == null)
            {
                throw new RIMDomainException("RIM:ErrorMessage:LocationNotChild")
                    .WithData("childLocationId", childLocationId)
                    .WithData("parentLocationId", Id);
            }

            _childLocations.Remove(hierarchyNode);
        }

        /// <summary>
        /// Gets a child location by ID
        /// </summary>
        /// <param name="childLocationId">Child location ID</param>
        /// <returns>The hierarchy node, or null if not found</returns>
        public LocationHierarchyNode GetChildLocation(Guid childLocationId)
        {
            return _childLocations.FirstOrDefault(cl => cl.ChildLocationId == childLocationId);
        }

        /// <summary>
        /// Updates the capacity information
        /// </summary>
        /// <param name="maxCapacity">Maximum capacity</param>
        /// <param name="capacityUnitOfMeasure">Unit of measure for capacity</param>
        public void UpdateCapacity(decimal maxCapacity, string capacityUnitOfMeasure)
        {
            if (maxCapacity < 0)
            {
                throw new RIMDomainException("RIM:Validation:CapacityNotNegative");
            }

            MaxCapacity = maxCapacity;
            CapacityUnitOfMeasure = capacityUnitOfMeasure;
        }

        private void SetCode(string code)
        {
            Code = Check.NotNullOrWhiteSpace(
                code,
                nameof(code),
                maxLength: Constants.RIMConstants.StorageLocations.MaxCodeLength
            );
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: Constants.RIMConstants.StorageLocations.MaxNameLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.Length(
                description,
                nameof(description),
                maxLength: Constants.RIMConstants.StorageLocations.MaxDescriptionLength
            );
        }
    }
}
