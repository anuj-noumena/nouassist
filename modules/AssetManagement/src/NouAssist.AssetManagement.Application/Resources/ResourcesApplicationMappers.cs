using NouAssist.AssetManagement.Resources.Aggregates;
using NouAssist.AssetManagement.Resources.Books.Dtos;
using NouAssist.AssetManagement.Resources.Collections.Dtos;
using NouAssist.AssetManagement.Resources.Entities;
using NouAssist.AssetManagement.Resources.Inventory.Dtos;
using NouAssist.AssetManagement.Resources.Resources.Dtos;
using NouAssist.AssetManagement.Resources.Storage.Dtos;
using NouAssist.AssetManagement.Resources.ValueObjects;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace NouAssist.AssetManagement.Resources
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ResourceToResourceDtoMapper : MapperBase<Resource, ResourceDto>
    {
        [MapperIgnoreTarget(nameof(ResourceDto.Kind))]
        [MapperIgnoreTarget(nameof(ResourceDto.ReadableSize))]
        public override partial ResourceDto Map(Resource source);

        public override partial void Map(Resource source, ResourceDto destination);

        public override void AfterMap(Resource source, ResourceDto destination)
        {
            destination.Kind = source.Kind.ToString();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ResourceVersionToResourceVersionDtoMapper : MapperBase<ResourceVersion, ResourceVersionDto>
    {
        [MapperIgnoreTarget(nameof(ResourceVersionDto.OcrStatus))]
        public override partial ResourceVersionDto Map(ResourceVersion source);

        public override partial void Map(ResourceVersion source, ResourceVersionDto destination);

        public override void AfterMap(ResourceVersion source, ResourceVersionDto destination)
        {
            destination.OcrStatus = source.OcrStatus.ToString();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ResourceAclToResourceAclDtoMapper : MapperBase<ResourceAcl, ResourceAclDto>
    {
        [MapperIgnoreTarget(nameof(ResourceAclDto.Permission))]
        public override partial ResourceAclDto Map(ResourceAcl source);

        public override partial void Map(ResourceAcl source, ResourceAclDto destination);

        public override void AfterMap(ResourceAcl source, ResourceAclDto destination)
        {
            destination.Permission = source.Permission.ToString();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class BookToBookDtoMapper : MapperBase<Book, BookDto>
    {
        [MapperIgnoreTarget(nameof(BookDto.ReadingStatus))]
        [MapperIgnoreTarget(nameof(BookDto.Format))]
        [MapperIgnoreTarget(nameof(BookDto.Source))]
        [MapperIgnoreTarget(nameof(BookDto.DisplayAuthors))]
        [MapperIgnoreTarget(nameof(BookDto.PrimaryIsbn))]
        [MapperIgnoreTarget(nameof(BookDto.ReadingProgressDisplay))]
        public override partial BookDto Map(Book source);

        public override partial void Map(Book source, BookDto destination);

        public override void AfterMap(Book source, BookDto destination)
        {
            destination.ReadingStatus = source.ReadingStatus.ToString();
            destination.Format = source.Format.ToString();
            destination.Source = source.Source.ToString();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class BookCollectionToBookCollectionDtoMapper : MapperBase<BookCollection, BookCollectionDto>
    {
        public override partial BookCollectionDto Map(BookCollection source);

        public override partial void Map(BookCollection source, BookCollectionDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class GoogleBookDataToGoogleBookDataDtoMapper : MapperBase<GoogleBookData, GoogleBookDataDto>
    {
        public override partial GoogleBookDataDto Map(GoogleBookData source);

        public override partial void Map(GoogleBookData source, GoogleBookDataDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class InventoryItemToInventoryItemDtoMapper : MapperBase<InventoryItem, InventoryItemDto>
    {
        [MapperIgnoreTarget(nameof(InventoryItemDto.Kind))]
        [MapperIgnoreTarget(nameof(InventoryItemDto.Condition))]
        [MapperIgnoreTarget(nameof(InventoryItemDto.Status))]
        [MapperIgnoreTarget(nameof(InventoryItemDto.DisplayLocation))]
        [MapperIgnoreTarget(nameof(InventoryItemDto.QuantityCheckedOut))]
        [MapperIgnoreTarget(nameof(InventoryItemDto.IsAvailable))]
        [MapperIgnoreTarget(nameof(InventoryItemDto.IsOverdue))]
        [MapperIgnoreTarget(nameof(InventoryItemDto.NeedsMaintenance))]
        public override partial InventoryItemDto Map(InventoryItem source);

        public override partial void Map(InventoryItem source, InventoryItemDto destination);

        public override void AfterMap(InventoryItem source, InventoryItemDto destination)
        {
            destination.Kind = source.Kind.ToString();
            destination.Condition = source.Condition.ToString();
            destination.Status = source.Status.ToString();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class StorageBackendToStorageBackendDtoMapper : MapperBase<StorageBackend, StorageBackendDto>
    {
        [MapperIgnoreTarget(nameof(StorageBackendDto.BackendType))]
        public override partial StorageBackendDto Map(StorageBackend source);

        public override partial void Map(StorageBackend source, StorageBackendDto destination);

        public override void AfterMap(StorageBackend source, StorageBackendDto destination)
        {
            destination.BackendType = source.BackendType.ToString();
        }
    }
}
