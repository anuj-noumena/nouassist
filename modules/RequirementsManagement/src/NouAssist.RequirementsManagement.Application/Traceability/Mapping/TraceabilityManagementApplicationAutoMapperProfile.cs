using AutoMapper;
using NouAssist.RequirementsManagement.Traceability.Aggregates;
using NouAssist.RequirementsManagement.Traceability.Artifact.Dtos;
using NouAssist.RequirementsManagement.Traceability.Entities;
using NouAssist.RequirementsManagement.Traceability.Impact.Dtos;
using NouAssist.RequirementsManagement.Traceability.Models;
using NouAssist.RequirementsManagement.Traceability.TraceLink.Dtos;
using NouAssist.RequirementsManagement.Traceability.TraceMatrix.Dtos;
using NouAssist.RequirementsManagement.Traceability.ValueObjects;

namespace NouAssist.RequirementsManagement.Traceability.Mapping
{
    /// <summary>
    /// AutoMapper profile for TraceabilityManagement module
    /// </summary>
    public class TraceabilityManagementApplicationAutoMapperProfile : Profile
    {
        public TraceabilityManagementApplicationAutoMapperProfile()
        {
            // TraceLink mappings
            CreateMap<TraceLink, TraceLinkDto>()
                .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => src.Identifier.Value));
                
            CreateMap<TraceableArtifactReference, TraceableArtifactDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ArtifactIdentifier.Id))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.ArtifactIdentifier.Type))
                .ForMember(dest => dest.Version, opt => opt.MapFrom(src => src.ArtifactIdentifier.Version))
                .ForMember(dest => dest.Uri, opt => opt.MapFrom(src => src.ArtifactIdentifier.Uri));
            
            // ArtifactIdentifier value object mappings
            CreateMap<CreateTraceLinkDto, ArtifactIdentifier>()
                .ConstructUsing(src => new ArtifactIdentifier(
                    src.SourceArtifactId,
                    src.SourceArtifactType,
                    src.SourceArtifactVersion,
                    src.SourceArtifactUri))
                .ForAllMembers(opt => opt.Ignore());
                
            CreateMap<CreateTraceLinkDto, ArtifactIdentifier>()
                .ConstructUsing(src => new ArtifactIdentifier(
                    src.TargetArtifactId,
                    src.TargetArtifactType,
                    src.TargetArtifactVersion,
                    src.TargetArtifactUri))
                .ForAllMembers(opt => opt.Ignore());
                
            // TraceabilityMatrix mappings
            CreateMap<TraceabilityMatrix, TraceabilityMatrixDto>()
                .ForMember(dest => dest.SourceArtifacts, opt => opt.Ignore())
                .ForMember(dest => dest.TargetArtifacts, opt => opt.Ignore())
                .ForMember(dest => dest.Cells, opt => opt.Ignore());
                
            // ImpactAnalysis mappings
            CreateMap<ImpactAnalysisResult, ImpactAnalysisResultDto>()
                .ForMember(dest => dest.ImpactSummaryByType, opt => opt.Ignore())
                .ForMember(dest => dest.ImpactSummaryByDepth, opt => opt.Ignore());
                
            CreateMap<ImpactedArtifact, ImpactedArtifactDto>()
                .ForMember(dest => dest.ArtifactName, opt => opt.Ignore());
                
            // CoverageReport mappings
            CreateMap<CoverageReport, CoverageReportDto>();
                
            // ArtifactInfo mappings
            CreateMap<ArtifactInfo, ArtifactInfoDto>();
        }
    }
}
