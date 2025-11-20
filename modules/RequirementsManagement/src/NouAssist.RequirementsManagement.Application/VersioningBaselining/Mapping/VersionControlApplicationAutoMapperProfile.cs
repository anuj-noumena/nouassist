using AutoMapper;
using NouAssist.RequirementsManagement.VersioningBaselining.Aggregates;
using NouAssist.RequirementsManagement.VersioningBaselining.Baselining.Dtos;
using NouAssist.RequirementsManagement.VersioningBaselining.Branching.Dtos;
using NouAssist.RequirementsManagement.VersioningBaselining.Entities;
using NouAssist.RequirementsManagement.VersioningBaselining.Services.Interfaces;
using NouAssist.RequirementsManagement.VersioningBaselining.ValueObjects;
using NouAssist.RequirementsManagement.VersioningBaselining.Versioning.Dtos;

namespace NouAssist.RequirementsManagement.VersioningBaselining.Mapping
{
    public class VersionControlApplicationAutoMapperProfile : Profile
    {
        public VersionControlApplicationAutoMapperProfile()
        {
            // Versioning mappings
            CreateMap<RequirementVersionRecord, VersionDto>()
                .ForMember(dest => dest.Version, opt => opt.MapFrom(src => src.Version.Value))
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.Author.Id))
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.Name))
                .ForMember(dest => dest.ChangeComment, opt => opt.MapFrom(src => src.ChangeComment.Text));
                
            CreateMap<RequirementVersionRecord, VersionRecordDto>()
                .ForMember(dest => dest.Version, opt => opt.MapFrom(src => src.Version.Value))
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.Author.Id))
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.Name))
                .ForMember(dest => dest.ChangeComment, opt => opt.MapFrom(src => src.ChangeComment.Text));
                
            CreateMap<VersionedRequirementHistory, VersionHistoryDto>()
                .ForMember(dest => dest.LatestVersion, opt => opt.MapFrom(src => src.LatestVersion.Value))
                .ForMember(dest => dest.Versions, opt => opt.MapFrom(src => src.VersionRecords));
                
            CreateMap<VersionComparisonResult, VersionComparisonResultDto>();

            // Baselining mappings
            CreateMap<BaselineDefinition, BaselineDto>();
            
            CreateMap<BaselineItem, BaselineItemDto>()
                .ForMember(dest => dest.Version, opt => opt.MapFrom(src => src.Version.Value));

            // Branching mappings
            CreateMap<RequirementBranch, BranchDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value))
                .ForMember(dest => dest.CreatedById, opt => opt.MapFrom(src => src.CreatedBy.Id))
                .ForMember(dest => dest.CreatedByName, opt => opt.MapFrom(src => src.CreatedBy.Name));
                
            CreateMap<MergeOperation, MergeOperationDto>()
                .ForMember(dest => dest.InitiatedById, opt => opt.MapFrom(src => src.InitiatedBy.Id))
                .ForMember(dest => dest.InitiatedByName, opt => opt.MapFrom(src => src.InitiatedBy.Name))
                .ForMember(dest => dest.CompletedById, opt => opt.MapFrom(src => src.CompletedBy != null ? src.CompletedBy.Id : (Guid?)null))
                .ForMember(dest => dest.CompletedByName, opt => opt.MapFrom(src => src.CompletedBy != null ? src.CompletedBy.Name : null));
                
            CreateMap<MergeResult, MergeResultDto>();
        }
    }
}
