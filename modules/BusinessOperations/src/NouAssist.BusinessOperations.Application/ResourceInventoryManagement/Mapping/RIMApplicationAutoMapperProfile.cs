using System;
using System.Linq;
using AutoMapper;
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

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Mapping
{
    /// <summary>
    /// AutoMapper profile for the RIM module
    /// </summary>
    public class RIMApplicationAutoMapperProfile : Profile
    {
        /// <summary>
        /// Creates a new instance of RIMApplicationAutoMapperProfile
        /// </summary>
        public RIMApplicationAutoMapperProfile()
        {
            /* Resource Type */
            CreateMap<ResourceTypeAggregate, ResourceTypeDto>()
                .ForMember(dest => dest.CategoryPath, opt => opt.MapFrom(src => src.CategoryPath.Path));

            CreateMap<ResourceAttributeDefinition, ResourceAttributeDefinitionDto>();
            
            /* Resource Instance */
            CreateMap<ResourceInstanceAggregate, ResourceInstanceDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId != null ? src.UserId.Value : (Guid?)null))
                .ForMember(dest => dest.AttributeValues, opt => opt.MapFrom(src => src.AttributeValues))
                .ForMember(dest => dest.CurrentAllocation, opt => opt.MapFrom(src => src.CurrentAllocation))
                .ForMember(dest => dest.ResourceTypeName, opt => opt.Ignore());

            CreateMap<AttributeValue, ResourceAttributeValueDto>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.StringValue));
            
            CreateMap<AllocationRecord, AllocationRecordDto>();
            CreateMap<UsageRecord, UsageRecordDto>();
            
            /* Inventory Item */
            CreateMap<InventoryItemDefinitionAggregate, InventoryItemDto>()
                .ForMember(dest => dest.Sku, opt => opt.MapFrom(src => src.Sku.Value))
                .ForMember(dest => dest.CategoryPath, opt => opt.MapFrom(src => src.CategoryPath.Path))
                .ForMember(dest => dest.TotalQuantity, opt => opt.MapFrom(src => src.GetTotalStockLevel()))
                .ForMember(dest => dest.StockLevels, opt => opt.MapFrom(src => src.StockLevels));

            CreateMap<StockLevel, StockLevelDto>()
                .ForMember(dest => dest.LocationName, opt => opt.Ignore())
                .ForMember(dest => dest.LocationCode, opt => opt.Ignore());
            
            CreateMap<InventoryTransaction, InventoryTransactionDto>()
                .ForMember(dest => dest.LocationName, opt => opt.Ignore());
            
            /* Resource Booking */
            CreateMap<ResourceBookingAggregate, ResourceBookingDto>()
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.TimeSlot.StartTime))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.TimeSlot.EndTime))
                .ForMember(dest => dest.ResourceName, opt => opt.Ignore())
                .ForMember(dest => dest.BookedByUserName, opt => opt.Ignore())
                .ForMember(dest => dest.AssignedToUserName, opt => opt.Ignore());
            
            /* Storage Location */
            CreateMap<StorageLocationAggregate, StorageLocationDto>()
                .ForMember(dest => dest.ParentLocationName, opt => opt.Ignore())
                .ForMember(dest => dest.Coordinates, opt => opt.MapFrom(src => 
                    src.Coordinates != null 
                        ? new LocationCoordinatesDto 
                        { 
                            Latitude = src.Coordinates.Latitude, 
                            Longitude = src.Coordinates.Longitude 
                        } 
                        : null))
                .ForMember(dest => dest.ChildLocations, opt => opt.Ignore());

            CreateMap<LocationHierarchyNode, LocationHierarchyNodeDto>();
            
            /* Inventory Audit */
            CreateMap<InventoryAuditSessionAggregate, InventoryAuditSessionDto>()
                .ForMember(dest => dest.LocationCode, opt => opt.Ignore())
                .ForMember(dest => dest.LocationName, opt => opt.Ignore())
                .ForMember(dest => dest.AuditedByUserName, opt => opt.Ignore())
                .ForMember(dest => dest.ItemsAudited, opt => opt.MapFrom(src => src.Discrepancies.Select(d => d.InventoryItemId).Distinct().Count()))
                .ForMember(dest => dest.DiscrepanciesFound, opt => opt.MapFrom(src => src.Discrepancies.Count));

            CreateMap<AuditDiscrepancy, AuditDiscrepancyDto>()
                .ForMember(dest => dest.ItemSku, opt => opt.Ignore())
                .ForMember(dest => dest.ItemName, opt => opt.Ignore());
            
            /* DTO to Entity mappings for create/update operations */
            
            // ResourceType
            CreateMap<CreateResourceTypeDto, ResourceTypeAggregate>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CategoryPath, opt => opt.MapFrom(src => 
                    !string.IsNullOrEmpty(src.CategoryPath) 
                        ? new CategoryPath(src.CategoryPath) 
                        : new CategoryPath(src.Category.ToString())))
                .ForMember(dest => dest.AttributeDefinitions, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore())
                .ForMember(dest => dest.ExtraProperties, opt => opt.Ignore())
                .ForMember(dest => dest.TenantId, opt => opt.Ignore());

            CreateMap<CreateResourceAttributeDefinitionDto, ResourceAttributeDefinition>()
                .ForMember(dest => dest.ResourceTypeId, opt => opt.Ignore())
                .ForMember(dest => dest.PossibleValues, opt => opt.MapFrom(src => 
                    src.PossibleValues != null && src.PossibleValues.Count > 0 
                        ? string.Join(",", src.PossibleValues) 
                        : null));
            
            // ResourceInstance
            CreateMap<CreateResourceInstanceDto, ResourceInstanceAggregate>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => 
                    src.UserId.HasValue 
                        ? new UserId(src.UserId.Value) 
                        : null))
                .ForMember(dest => dest.ActualRetirementDate, opt => opt.Ignore())
                .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore())
                .ForMember(dest => dest.ExtraProperties, opt => opt.Ignore())
                .ForMember(dest => dest.TenantId, opt => opt.Ignore());
            
            // InventoryItem
            CreateMap<CreateInventoryItemDto, InventoryItemDefinitionAggregate>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Sku, opt => opt.MapFrom(src => new StockKeepingUnit(src.Sku)))
                .ForMember(dest => dest.CategoryPath, opt => opt.MapFrom(src => 
                    !string.IsNullOrEmpty(src.CategoryPath) 
                        ? new CategoryPath(src.CategoryPath) 
                        : new CategoryPath("Uncategorized")))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.StockLevels, opt => opt.Ignore())
                .ForMember(dest => dest.Transactions, opt => opt.Ignore())
                .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore())
                .ForMember(dest => dest.ExtraProperties, opt => opt.Ignore())
                .ForMember(dest => dest.TenantId, opt => opt.Ignore());
            
            // ResourceBooking
            CreateMap<CreateResourceBookingDto, ResourceBookingAggregate>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BookingCode, opt => opt.Ignore())
                .ForMember(dest => dest.TimeSlot, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.CancellationReason, opt => opt.Ignore())
                .ForMember(dest => dest.CheckedOutAt, opt => opt.Ignore())
                .ForMember(dest => dest.CheckedInAt, opt => opt.Ignore())
                .ForMember(dest => dest.BookedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore())
                .ForMember(dest => dest.ExtraProperties, opt => opt.Ignore())
                .ForMember(dest => dest.TenantId, opt => opt.Ignore());
            
            // StorageLocation
            CreateMap<CreateStorageLocationDto, StorageLocationAggregate>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Coordinates, opt => opt.MapFrom(src => 
                    src.Coordinates != null 
                        ? new LocationCoordinates(src.Coordinates.Latitude, src.Coordinates.Longitude) 
                        : null))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.ChildLocations, opt => opt.Ignore())
                .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore())
                .ForMember(dest => dest.ExtraProperties, opt => opt.Ignore())
                .ForMember(dest => dest.TenantId, opt => opt.Ignore());
            
            // InventoryAudit
            CreateMap<CreateInventoryAuditSessionDto, InventoryAuditSessionAggregate>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => AuditStatus.Planned))
                .ForMember(dest => dest.ActualStartTime, opt => opt.Ignore())
                .ForMember(dest => dest.CompletionTime, opt => opt.Ignore())
                .ForMember(dest => dest.AuditedByUserId, opt => opt.MapFrom(src => CurrentUser.Id))
                .ForMember(dest => dest.Discrepancies, opt => opt.Ignore())
                .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore())
                .ForMember(dest => dest.ExtraProperties, opt => opt.Ignore())
                .ForMember(dest => dest.TenantId, opt => opt.Ignore());
        }
    }
}
