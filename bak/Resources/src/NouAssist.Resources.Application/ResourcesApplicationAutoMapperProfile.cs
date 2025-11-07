using AutoMapper;
using NouAssist.Resources.Aggregates;
using NouAssist.Resources.Books.Dtos;
using NouAssist.Resources.Collections.Dtos;
using NouAssist.Resources.Entities;
using NouAssist.Resources.Inventory.Dtos;
using NouAssist.Resources.Resources.Dtos;
using NouAssist.Resources.Storage.Dtos;
using NouAssist.Resources.ValueObjects;

namespace NouAssist.Resources
{
    public class ResourcesApplicationAutoMapperProfile : Profile
    {
        public ResourcesApplicationAutoMapperProfile()
        {
            // Resource mappings
            CreateMap<Resource, ResourceDto>()
                .ForMember(dest => dest.Kind, opt => opt.MapFrom(src => src.Kind.ToString()))
                .ForMember(dest => dest.ReadableSize, opt => opt.Ignore()); // Computed in service

            CreateMap<ResourceVersion, ResourceVersionDto>()
                .ForMember(dest => dest.OcrStatus, opt => opt.MapFrom(src => src.OcrStatus.ToString()));

            CreateMap<ResourceAcl, ResourceAclDto>()
                .ForMember(dest => dest.Permission, opt => opt.MapFrom(src => src.Permission.ToString()));

            // Book mappings
            CreateMap<Book, BookDto>()
                .ForMember(dest => dest.ReadingStatus, opt => opt.MapFrom(src => src.ReadingStatus.ToString()))
                .ForMember(dest => dest.Format, opt => opt.MapFrom(src => src.Format.ToString()))
                .ForMember(dest => dest.Source, opt => opt.MapFrom(src => src.Source.ToString()))
                .ForMember(dest => dest.DisplayAuthors, opt => opt.Ignore())
                .ForMember(dest => dest.PrimaryIsbn, opt => opt.Ignore())
                .ForMember(dest => dest.ReadingProgressDisplay, opt => opt.Ignore());

            CreateMap<BookCollection, BookCollectionDto>();
            
            CreateMap<GoogleBookData, GoogleBookDataDto>();

            // Inventory mappings
            CreateMap<InventoryItem, InventoryItemDto>()
                .ForMember(dest => dest.Kind, opt => opt.MapFrom(src => src.Kind.ToString()))
                .ForMember(dest => dest.Condition, opt => opt.MapFrom(src => src.Condition.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.DisplayLocation, opt => opt.Ignore())
                .ForMember(dest => dest.QuantityCheckedOut, opt => opt.Ignore())
                .ForMember(dest => dest.IsAvailable, opt => opt.Ignore())
                .ForMember(dest => dest.IsOverdue, opt => opt.Ignore())
                .ForMember(dest => dest.NeedsMaintenance, opt => opt.Ignore());
            
            CreateMap<StorageBackend, StorageBackendDto>()
                .ForMember(dest => dest.BackendType, opt => opt.MapFrom(src => src.BackendType.ToString()));
            
        }
    }
}
