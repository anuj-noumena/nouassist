using AutoMapper;
using NouAssist.RequirementsManagement.ChangeManagement.Aggregates;
using NouAssist.RequirementsManagement.ChangeManagement.ChangeRequest.Dtos;
using NouAssist.RequirementsManagement.ChangeManagement.Entities;
using NouAssist.RequirementsManagement.ChangeManagement.ValueObjects;
using NouAssist.RequirementsManagement.ChangeManagement.Services;

namespace NouAssist.RequirementsManagement.ChangeManagement.Mapping
{
    public class ChangeManagementApplicationAutoMapperProfile : Profile
    {
        public ChangeManagementApplicationAutoMapperProfile()
        {
            // --- Aggregate <-> DTO Mappings ---
            
            // Main change request mapping
            CreateMap<RequirementChangeRequest, ChangeRequestDto>()
                .ForMember(dest => dest.RequestIdentifier, 
                    opt => opt.MapFrom(src => src.RequestIdentifier.Value))
                .ForMember(dest => dest.AffectedRequirement, 
                    opt => opt.MapFrom(src => src.AffectedRequirement))
                .ForMember(dest => dest.SubmitterName, opt => opt.Ignore())
                .ForMember(dest => dest.CanEdit, opt => opt.Ignore())
                .ForMember(dest => dest.TotalCostImpact, opt => opt.Ignore())
                .ForMember(dest => dest.TotalTimeImpact, opt => opt.Ignore());
            
            // Value Object mappings
            CreateMap<AffectedRequirementRef, AffectedRequirementDto>()
                .ForMember(dest => dest.DisplayName, opt => opt.Ignore());
            
            CreateMap<ImpactDetails, ImpactDetailsDto>();
            
            // Entity mappings
            CreateMap<ChangeRationale, ChangeRationaleDto>();
            
            CreateMap<ChangeImpactStatement, ChangeImpactStatementDto>();
            
            CreateMap<ApprovalStep, ApprovalStepDto>()
                .ForMember(dest => dest.ApproverRoleName, 
                    opt => opt.MapFrom(src => src.ApproverRole.Name))
                .ForMember(dest => dest.ApproverRolePriority, 
                    opt => opt.MapFrom(src => src.ApproverRole.Priority))
                .ForMember(dest => dest.DecisionReason, 
                    opt => opt.MapFrom(src => src.DecisionReason != null ? src.DecisionReason.Value : null))
                .ForMember(dest => dest.ApproverName, opt => opt.Ignore());
            
            // DTO mappings for impact assessment
            CreateMap<ImpactAssessmentResult, ImpactAssessmentResultDto>();
            CreateMap<DependencyAnalysisResult, DependencyAnalysisResultDto>();
            
            // --- Input DTO for creating domain objects ---
            CreateMap<CreateChangeRequestDto, RequirementChangeRequest>()
                .Ignore(x => x.Id)
                .Ignore(x => x.RequestIdentifier)
                .Ignore(x => x.AffectedRequirement)
                .Ignore(x => x.Status)
                .Ignore(x => x.SubmitterId)
                .Ignore(x => x.SubmissionDate)
                .Ignore(x => x.CompletionDate)
                .IgnoreAuditingProperties();
            
            CreateMap<CreateChangeRationaleDto, ChangeRationale>()
                .Ignore(x => x.Id)
                .Ignore(x => x.ChangeRequestId)
                .Ignore(x => x.CreatedDate);
            
            CreateMap<AddImpactStatementDto, ChangeImpactStatement>()
                .Ignore(x => x.Id)
                .Ignore(x => x.ChangeRequestId)
                .Ignore(x => x.ImpactDetails)
                .Ignore(x => x.AssessmentDate)
                .IgnoreAuditingProperties();
        }
    }

    // Extension to ignore auditing properties
    internal static class IgnoreAuditingPropertiesExtensions
    {
        public static IMappingExpression<TSource, TDestination> IgnoreAuditingProperties<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> expression)
        {
            expression
                .Ignore(x => ((dynamic)x).CreationTime)
                .Ignore(x => ((dynamic)x).CreatorId)
                .Ignore(x => ((dynamic)x).LastModificationTime)
                .Ignore(x => ((dynamic)x).LastModifierId)
                .Ignore(x => ((dynamic)x).IsDeleted)
                .Ignore(x => ((dynamic)x).DeleterId)
                .Ignore(x => ((dynamic)x).DeletionTime);

            return expression;
        }
    }
}
