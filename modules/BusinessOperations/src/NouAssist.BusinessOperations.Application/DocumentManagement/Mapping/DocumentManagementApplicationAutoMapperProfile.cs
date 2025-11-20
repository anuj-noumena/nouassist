using AutoMapper;
using NouAssist.BusinessOperations.DocumentManagement.Aggregates;
using NouAssist.BusinessOperations.DocumentManagement.Documents.Dtos;
using NouAssist.BusinessOperations.DocumentManagement.Entities;
using NouAssist.BusinessOperations.DocumentManagement.ValueObjects;

namespace NouAssist.BusinessOperations.DocumentManagement.Mapping
{
    public class DocumentManagementApplicationAutoMapperProfile : Profile
    {
        public DocumentManagementApplicationAutoMapperProfile()
        {
            // Document -> DocumentDto
            CreateMap<Document, DocumentDto>()
                .ForMember(dest => dest.Filename, opt => opt.MapFrom(src => src.Metadata.Filename))
                .ForMember(dest => dest.MimeType, opt => opt.MapFrom(src => src.Metadata.MimeType))
                .ForMember(dest => dest.FileSize, opt => opt.MapFrom(src => src.Metadata.FileSize))
                .ForMember(dest => dest.UploadedByUserId, opt => opt.MapFrom(src => src.Metadata.UploadedByUserId))
                .ForMember(dest => dest.UploadedAt, opt => opt.MapFrom(src => src.Metadata.UploadedAt))
                .ForMember(dest => dest.VersionNumber, opt => opt.MapFrom(src => src.CurrentVersionNumber.ToString()))
                .ForMember(dest => dest.VersionCount, opt => opt.MapFrom(src => src.Versions.Count));
                
            // AccessGrant -> DocumentAccessGrantDto
            CreateMap<AccessGrant, DocumentAccessGrantDto>();
            
            // DocumentVersion -> DocumentVersionDto
            CreateMap<DocumentVersion, DocumentVersionDto>()
                .ForMember(dest => dest.VersionNumber, opt => opt.MapFrom(src => src.VersionNumber.ToString()))
                .ForMember(dest => dest.Filename, opt => opt.MapFrom(src => src.Metadata.Filename))
                .ForMember(dest => dest.MimeType, opt => opt.MapFrom(src => src.Metadata.MimeType))
                .ForMember(dest => dest.FileSize, opt => opt.MapFrom(src => src.Metadata.FileSize))
                .ForMember(dest => dest.CreatedByUserId, opt => opt.MapFrom(src => src.Metadata.UploadedByUserId))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.Metadata.UploadedAt));
        }
    }
}
