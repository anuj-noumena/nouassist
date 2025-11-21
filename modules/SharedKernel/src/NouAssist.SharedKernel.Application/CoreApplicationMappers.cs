using NouAssist.SharedKernel.Aggregates;
using NouAssist.SharedKernel.Areas.Dtos;
using NouAssist.SharedKernel.Contexts.Dtos;
using NouAssist.SharedKernel.Inbox.Dtos;
using NouAssist.SharedKernel.Tags.Dtos;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace NouAssist.SharedKernel
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class TagToTagDtoMapper : MapperBase<Tag, TagDto>
    {
        [MapperIgnoreTarget(nameof(TagDto.IsPopular))]
        public override partial TagDto Map(Tag source);

        [MapperIgnoreTarget(nameof(TagDto.IsPopular))]
        public override partial void Map(Tag source, TagDto destination);

        public override void AfterMap(Tag source, TagDto destination)
        {
            destination.IsPopular = source.IsPopular;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class TagCategoryToTagCategoryDtoMapper : MapperBase<TagCategory, TagCategoryDto>
    {
        public override partial TagCategoryDto Map(TagCategory source);
        public override partial void Map(TagCategory source, TagCategoryDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ContextToContextDtoMapper : MapperBase<Context, ContextDto>
    {
        [MapperIgnoreTarget(nameof(ContextDto.IsArchived))]
        public override partial ContextDto Map(Context source);

        [MapperIgnoreTarget(nameof(ContextDto.IsArchived))]
        public override partial void Map(Context source, ContextDto destination);

        public override void AfterMap(Context source, ContextDto destination)
        {
            destination.IsArchived = source.IsArchived;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class AreaToAreaDtoMapper : MapperBase<Area, AreaDto>
    {
        [MapperIgnoreTarget(nameof(AreaDto.HasParent))]
        [MapperIgnoreTarget(nameof(AreaDto.IsRootArea))]
        [MapperIgnoreTarget(nameof(AreaDto.IsArchived))]
        public override partial AreaDto Map(Area source);

        [MapperIgnoreTarget(nameof(AreaDto.HasParent))]
        [MapperIgnoreTarget(nameof(AreaDto.IsRootArea))]
        [MapperIgnoreTarget(nameof(AreaDto.IsArchived))]
        public override partial void Map(Area source, AreaDto destination);

        public override void AfterMap(Area source, AreaDto destination)
        {
            destination.HasParent = source.HasParent;
            destination.IsRootArea = source.IsRootArea;
            destination.IsArchived = source.IsArchived;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class AreaToAreaWithChildrenDtoMapper : MapperBase<Area, AreaWithChildrenDto>
    {
        private readonly AreaToAreaDtoMapper _baseMapper;

        public AreaToAreaWithChildrenDtoMapper(AreaToAreaDtoMapper baseMapper)
        {
            _baseMapper = baseMapper;
        }

        [MapperIgnoreTarget(nameof(AreaWithChildrenDto.HasParent))]
        [MapperIgnoreTarget(nameof(AreaWithChildrenDto.IsRootArea))]
        [MapperIgnoreTarget(nameof(AreaWithChildrenDto.IsArchived))]
        public override partial AreaWithChildrenDto Map(Area source);

        [MapperIgnoreTarget(nameof(AreaWithChildrenDto.HasParent))]
        [MapperIgnoreTarget(nameof(AreaWithChildrenDto.IsRootArea))]
        [MapperIgnoreTarget(nameof(AreaWithChildrenDto.IsArchived))]
        public override partial void Map(Area source, AreaWithChildrenDto destination);

        public override void AfterMap(Area source, AreaWithChildrenDto destination)
        {
            // Apply base mapping logic
            destination.HasParent = source.HasParent;
            destination.IsRootArea = source.IsRootArea;
            destination.IsArchived = source.IsArchived;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class InboxItemToInboxItemDtoMapper : MapperBase<InboxItem, InboxItemDto>
    {
        [MapperIgnoreTarget(nameof(InboxItemDto.AgeInDays))]
        [MapperIgnoreTarget(nameof(InboxItemDto.IsStale))]
        public override partial InboxItemDto Map(InboxItem source);

        [MapperIgnoreTarget(nameof(InboxItemDto.AgeInDays))]
        [MapperIgnoreTarget(nameof(InboxItemDto.IsStale))]
        public override partial void Map(InboxItem source, InboxItemDto destination);

        public override void AfterMap(InboxItem source, InboxItemDto destination)
        {
            destination.AgeInDays = source.GetAgeInDays();
            destination.IsStale = source.IsStale();
        }
    }
}
