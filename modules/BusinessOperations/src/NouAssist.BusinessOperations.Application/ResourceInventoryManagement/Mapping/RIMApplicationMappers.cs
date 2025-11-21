using System;
using System.Linq;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.InventoryAudit;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.InventoryItem;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.ResourceBooking;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.ResourceInstance;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.ResourceType;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.StorageLocation;
using NouAssist.BusinessOperations.ResourceInventoryManagement.InventoryAudit.Dtos;
using NouAssist.BusinessOperations.ResourceInventoryManagement.InventoryItem.Dtos;
using NouAssist.BusinessOperations.ResourceInventoryManagement.ResourceBooking.Dtos;
using NouAssist.BusinessOperations.ResourceInventoryManagement.ResourceInstance.Dtos;
using NouAssist.BusinessOperations.ResourceInventoryManagement.ResourceType.Dtos;
using NouAssist.BusinessOperations.ResourceInventoryManagement.StorageLocation.Dtos;
using NouAssist.BusinessOperations.ResourceInventoryManagement.ValueObjects;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Mapping
{
    /* Resource Type Mappers */

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ResourceTypeAggregateToResourceTypeDtoMapper : MapperBase<ResourceTypeAggregate, ResourceTypeDto>
    {
        public override partial ResourceTypeDto Map(ResourceTypeAggregate source);
        public override partial void Map(ResourceTypeAggregate source, ResourceTypeDto destination);

        public override void AfterMap(ResourceTypeAggregate source, ResourceTypeDto destination)
        {
            destination.CategoryPath = source.CategoryPath.Path;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ResourceAttributeDefinitionToResourceAttributeDefinitionDtoMapper : MapperBase<ResourceAttributeDefinition, ResourceAttributeDefinitionDto>
    {
        public override partial ResourceAttributeDefinitionDto Map(ResourceAttributeDefinition source);
        public override partial void Map(ResourceAttributeDefinition source, ResourceAttributeDefinitionDto destination);
    }

    /* Resource Instance Mappers */

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ResourceInstanceAggregateToResourceInstanceDtoMapper : MapperBase<ResourceInstanceAggregate, ResourceInstanceDto>
    {
        [MapperIgnoreTarget(nameof(ResourceInstanceDto.ResourceTypeName))]
        public override partial ResourceInstanceDto Map(ResourceInstanceAggregate source);

        public override partial void Map(ResourceInstanceAggregate source, ResourceInstanceDto destination);

        public override void AfterMap(ResourceInstanceAggregate source, ResourceInstanceDto destination)
        {
            destination.UserId = source.UserId != null ? source.UserId.Value : (Guid?)null;
            destination.AttributeValues = source.AttributeValues.Select(av => new ResourceAttributeValueDto
            {
                AttributeDefinitionId = av.AttributeDefinitionId,
                Value = av.StringValue
            }).ToList();
            destination.CurrentAllocation = source.CurrentAllocation != null
                ? new AllocationRecordDto
                {
                    Id = source.CurrentAllocation.Id,
                    AllocationPercentage = source.CurrentAllocation.AllocationPercentage,
                    AllocatedFromDate = source.CurrentAllocation.AllocatedFromDate,
                    AllocatedToDate = source.CurrentAllocation.AllocatedToDate,
                    ProjectId = source.CurrentAllocation.ProjectId,
                    TaskId = source.CurrentAllocation.TaskId
                }
                : null;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class AttributeValueToResourceAttributeValueDtoMapper : MapperBase<AttributeValue, ResourceAttributeValueDto>
    {
        public override partial ResourceAttributeValueDto Map(AttributeValue source);
        public override partial void Map(AttributeValue source, ResourceAttributeValueDto destination);

        public override void AfterMap(AttributeValue source, ResourceAttributeValueDto destination)
        {
            destination.Value = source.StringValue;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class AllocationRecordToAllocationRecordDtoMapper : MapperBase<AllocationRecord, AllocationRecordDto>
    {
        public override partial AllocationRecordDto Map(AllocationRecord source);
        public override partial void Map(AllocationRecord source, AllocationRecordDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class UsageRecordToUsageRecordDtoMapper : MapperBase<UsageRecord, UsageRecordDto>
    {
        public override partial UsageRecordDto Map(UsageRecord source);
        public override partial void Map(UsageRecord source, UsageRecordDto destination);
    }

    /* Inventory Item Mappers */

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class InventoryItemDefinitionAggregateToInventoryItemDtoMapper : MapperBase<InventoryItemDefinitionAggregate, InventoryItemDto>
    {
        public override partial InventoryItemDto Map(InventoryItemDefinitionAggregate source);
        public override partial void Map(InventoryItemDefinitionAggregate source, InventoryItemDto destination);

        public override void AfterMap(InventoryItemDefinitionAggregate source, InventoryItemDto destination)
        {
            destination.Sku = source.Sku.Value;
            destination.CategoryPath = source.CategoryPath.Path;
            destination.TotalQuantity = source.GetTotalStockLevel();
            destination.StockLevels = source.StockLevels.Select(sl => new StockLevelDto
            {
                Id = sl.Id,
                InventoryItemId = sl.InventoryItemId,
                LocationId = sl.LocationId,
                Quantity = sl.Quantity,
                ReorderPoint = sl.ReorderPoint,
                MaximumStock = sl.MaximumStock,
                LastRestocked = sl.LastRestocked
            }).ToList();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class StockLevelToStockLevelDtoMapper : MapperBase<StockLevel, StockLevelDto>
    {
        [MapperIgnoreTarget(nameof(StockLevelDto.LocationName))]
        [MapperIgnoreTarget(nameof(StockLevelDto.LocationCode))]
        public override partial StockLevelDto Map(StockLevel source);

        public override partial void Map(StockLevel source, StockLevelDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class InventoryTransactionToInventoryTransactionDtoMapper : MapperBase<InventoryTransaction, InventoryTransactionDto>
    {
        [MapperIgnoreTarget(nameof(InventoryTransactionDto.LocationName))]
        public override partial InventoryTransactionDto Map(InventoryTransaction source);

        public override partial void Map(InventoryTransaction source, InventoryTransactionDto destination);
    }

    /* Resource Booking Mappers */

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ResourceBookingAggregateToResourceBookingDtoMapper : MapperBase<ResourceBookingAggregate, ResourceBookingDto>
    {
        [MapperIgnoreTarget(nameof(ResourceBookingDto.ResourceName))]
        [MapperIgnoreTarget(nameof(ResourceBookingDto.BookedByUserName))]
        [MapperIgnoreTarget(nameof(ResourceBookingDto.AssignedToUserName))]
        public override partial ResourceBookingDto Map(ResourceBookingAggregate source);

        public override partial void Map(ResourceBookingAggregate source, ResourceBookingDto destination);

        public override void AfterMap(ResourceBookingAggregate source, ResourceBookingDto destination)
        {
            destination.StartTime = source.TimeSlot.StartTime;
            destination.EndTime = source.TimeSlot.EndTime;
        }
    }

    /* Storage Location Mappers */

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class StorageLocationAggregateToStorageLocationDtoMapper : MapperBase<StorageLocationAggregate, StorageLocationDto>
    {
        [MapperIgnoreTarget(nameof(StorageLocationDto.ParentLocationName))]
        [MapperIgnoreTarget(nameof(StorageLocationDto.ChildLocations))]
        public override partial StorageLocationDto Map(StorageLocationAggregate source);

        public override partial void Map(StorageLocationAggregate source, StorageLocationDto destination);

        public override void AfterMap(StorageLocationAggregate source, StorageLocationDto destination)
        {
            destination.Coordinates = source.Coordinates != null
                ? new LocationCoordinatesDto
                {
                    Latitude = source.Coordinates.Latitude,
                    Longitude = source.Coordinates.Longitude
                }
                : null;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class LocationHierarchyNodeToLocationHierarchyNodeDtoMapper : MapperBase<LocationHierarchyNode, LocationHierarchyNodeDto>
    {
        public override partial LocationHierarchyNodeDto Map(LocationHierarchyNode source);
        public override partial void Map(LocationHierarchyNode source, LocationHierarchyNodeDto destination);
    }

    /* Inventory Audit Mappers */

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class InventoryAuditSessionAggregateToInventoryAuditSessionDtoMapper : MapperBase<InventoryAuditSessionAggregate, InventoryAuditSessionDto>
    {
        [MapperIgnoreTarget(nameof(InventoryAuditSessionDto.LocationCode))]
        [MapperIgnoreTarget(nameof(InventoryAuditSessionDto.LocationName))]
        [MapperIgnoreTarget(nameof(InventoryAuditSessionDto.AuditedByUserName))]
        public override partial InventoryAuditSessionDto Map(InventoryAuditSessionAggregate source);

        public override partial void Map(InventoryAuditSessionAggregate source, InventoryAuditSessionDto destination);

        public override void AfterMap(InventoryAuditSessionAggregate source, InventoryAuditSessionDto destination)
        {
            destination.ItemsAudited = source.Discrepancies.Select(d => d.InventoryItemId).Distinct().Count();
            destination.DiscrepanciesFound = source.Discrepancies.Count;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class AuditDiscrepancyToAuditDiscrepancyDtoMapper : MapperBase<AuditDiscrepancy, AuditDiscrepancyDto>
    {
        [MapperIgnoreTarget(nameof(AuditDiscrepancyDto.ItemSku))]
        [MapperIgnoreTarget(nameof(AuditDiscrepancyDto.ItemName))]
        public override partial AuditDiscrepancyDto Map(AuditDiscrepancy source);

        public override partial void Map(AuditDiscrepancy source, AuditDiscrepancyDto destination);
    }

    /* DTO to Entity Mappers for Create/Update Operations */

    // Note: The original AutoMapper profile had create mappings that used CurrentUser.Id and other runtime dependencies.
    // These complex mappings with constructor calls and conditional logic are typically handled in the application service layer
    // rather than in mappers, as Mapperly doesn't support runtime dependency injection in mappers.
    // The application service layer should handle the creation of value objects and aggregates with proper validation.
}
