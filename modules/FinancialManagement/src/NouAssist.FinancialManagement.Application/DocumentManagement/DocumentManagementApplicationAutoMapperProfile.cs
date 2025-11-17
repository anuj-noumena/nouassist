using AutoMapper;
using NouAssist.DocumentManagement.Dtos;
using NouAssist.FinancialManagement.DocumentManagement.Aggregates;
using NouAssist.FinancialManagement.DocumentManagement.ValueObjects;

namespace NouAssist.DocumentManagement
{
    public class DocumentManagementApplicationAutoMapperProfile : Profile
    {
        public DocumentManagementApplicationAutoMapperProfile()
        {
            CreateMap<Document, DocumentDto>()
                .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.Metadata.FileName))
                .ForMember(dest => dest.ContentType, opt => opt.MapFrom(src => src.Metadata.ContentType))
                .ForMember(dest => dest.FileSize, opt => opt.MapFrom(src => src.Metadata.FileSize))
                .ForMember(dest => dest.FileExtension, opt => opt.MapFrom(src => src.Metadata.FileExtension))
                .ForMember(dest => dest.EntityType, opt => opt.MapFrom(src => src.EntityReference != null ? src.EntityReference.EntityType : null))
                .ForMember(dest => dest.EntityId, opt => opt.MapFrom(src => src.EntityReference != null ? src.EntityReference.EntityId : null))
                .ForMember(dest => dest.OCRResult, opt => opt.MapFrom(src => src.OCRResult));

            CreateMap<OCRResult, OCRResultDto>();

            CreateMap<DocumentFolder, DocumentFolderDto>()
                .ForMember(dest => dest.DocumentCount, opt => opt.MapFrom(src => src.DocumentIds.Count));
        }
    }
}
