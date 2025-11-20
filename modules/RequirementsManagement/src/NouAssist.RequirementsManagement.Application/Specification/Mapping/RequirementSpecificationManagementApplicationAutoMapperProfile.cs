using AutoMapper;
using NouAssist.RequirementsManagement.Specification.Aggregates;
using NouAssist.RequirementsManagement.Specification.Entities;
using NouAssist.RequirementsManagement.Specification.Requirements.Dtos;
using NouAssist.RequirementsManagement.Specification.SpecificationStructures.Dtos;
using NouAssist.RequirementsManagement.Specification.ValueObjects;

namespace NouAssist.RequirementsManagement.Specification.Mapping
{
    public class RequirementSpecificationManagementApplicationAutoMapperProfile : Profile
    {
        public RequirementSpecificationManagementApplicationAutoMapperProfile()
        {
            // Requirement aggregate mappings
            CreateMap<Requirement, RequirementDto>()
                .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => src.Identifier.Value))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Text.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Text.Description));

            // Related entity mappings
            CreateMap<AcceptanceCriterion, AcceptanceCriterionDto>()
                .ForMember(dest => dest.Statement, opt => opt.MapFrom(src => src.Statement.Statement))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Statement.Description))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Statement.Type));

            CreateMap<AssociatedRule, AssociatedRuleDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Rule.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Rule.Description));

            CreateMap<AssociatedConstraint, AssociatedConstraintDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Constraint.Name))
                .ForMember(dest => dest.Statement, opt => opt.MapFrom(src => src.Constraint.Statement))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Constraint.Type));

            CreateMap<DataModelReference, DataModelReferenceDto>()
                .ForMember(dest => dest.FieldName, opt => opt.MapFrom(src => src.FieldSpecification.FieldName))
                .ForMember(dest => dest.DataType, opt => opt.MapFrom(src => src.FieldSpecification.DataType))
                .ForMember(dest => dest.IsRequired, opt => opt.MapFrom(src => src.FieldSpecification.IsRequired))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.FieldSpecification.Description))
                .ForMember(dest => dest.MaxLength, opt => opt.MapFrom(src => src.FieldSpecification.MaxLength));

            CreateMap<InterfaceSpecification, InterfaceSpecificationDto>()
                .ForMember(dest => dest.InterfaceName, opt => opt.MapFrom(src => src.Signature.InterfaceName))
                .ForMember(dest => dest.Endpoint, opt => opt.MapFrom(src => src.Signature.Endpoint))
                .ForMember(dest => dest.HttpMethod, opt => opt.MapFrom(src => src.Signature.HttpMethod))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Signature.Type))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Signature.Description));

            CreateMap<BusinessObjectiveLink, BusinessObjectiveLinkDto>()
                .ForMember(dest => dest.ObjectiveId, opt => opt.MapFrom(src => src.ObjectiveId.Value));

            CreateMap<RequirementScopeMarker, RequirementScopeMarkerDto>();

            // Specification structure mappings
            CreateMap<SpecificationStructure, SpecificationStructureDto>()
                .ForMember(dest => dest.SectionsCount, opt => opt.MapFrom(src => src.Sections.Count));

            CreateMap<SpecificationSection, SpecificationSectionDto>();

            // Value object mappings (typically not needed as they're flattened, but included for completeness)
            CreateMap<RequirementIdentifier, string>().ConvertUsing(src => src.Value);
            CreateMap<string, RequirementIdentifier>().ConvertUsing(src => new RequirementIdentifier(src));

            CreateMap<RequirementText, object>().ConvertUsing(src => new { src.Title, src.Description });

            CreateMap<CriterionStatement, object>().ConvertUsing(src => new { src.Statement, src.Description, src.Type });

            CreateMap<RuleDescription, object>().ConvertUsing(src => new { src.Name, src.Description });

            CreateMap<ConstraintStatement, object>().ConvertUsing(src => new { src.Name, src.Statement, src.Type });

            CreateMap<DataFieldSpec, object>().ConvertUsing(src => new { src.FieldName, src.DataType, src.IsRequired, src.Description, src.MaxLength });

            CreateMap<ApiSignature, object>().ConvertUsing(src => new { src.InterfaceName, src.Endpoint, src.HttpMethod, src.Type, src.Description });

            CreateMap<ObjectiveId, Guid>().ConvertUsing(src => src.Value);
            CreateMap<Guid, ObjectiveId>().ConvertUsing(src => new ObjectiveId(src));
        }
    }
}
