using AutoMapper;
using NouAssist.Workflow.Aggregates;
using NouAssist.Workflow.Entities;
using NouAssist.Workflow.Workflows.Dtos;

namespace NouAssist.Workflow;

public class WorkflowApplicationAutoMapperProfile : Profile
{
    public WorkflowApplicationAutoMapperProfile()
    {
        // Workflow Aggregate
        CreateMap<Aggregates.Workflow, WorkflowDto>()
            .ForMember(dest => dest.Statuses, opt => opt.MapFrom(src => src.Statuses))
            .ForMember(dest => dest.Transitions, opt => opt.MapFrom(src => src.Transitions));

        // WorkflowStatus Entity
        CreateMap<WorkflowStatus, WorkflowStatusDto>()
            .ForMember(dest => dest.IsTodo, opt => opt.MapFrom(src => src.IsTodo))
            .ForMember(dest => dest.IsInProgress, opt => opt.MapFrom(src => src.IsInProgress))
            .ForMember(dest => dest.IsDone, opt => opt.MapFrom(src => src.IsDone))
            .ForMember(dest => dest.IsCancelled, opt => opt.MapFrom(src => src.IsCancelled))
            .ForMember(dest => dest.IsBlocked, opt => opt.MapFrom(src => src.IsBlocked))
            .ForMember(dest => dest.IsTerminal, opt => opt.MapFrom(src => src.IsTerminal));

        // WorkflowTransition Entity
        CreateMap<WorkflowTransition, WorkflowTransitionDto>()
            .ForMember(dest => dest.AllowedRoles, opt => opt.MapFrom(src => src.AllowedRoles))
            .ForMember(dest => dest.AllowedUserIds, opt => opt.MapFrom(src => src.AllowedUserIds))
            .ForMember(dest => dest.HasConditions, opt => opt.MapFrom(src => src.HasConditions))
            .ForMember(dest => dest.HasValidators, opt => opt.MapFrom(src => src.HasValidators))
            .ForMember(dest => dest.HasRestrictions, opt => opt.MapFrom(src => src.HasRestrictions))
            .ForMember(dest => dest.Actions, opt => opt.MapFrom(src => src.Actions));

        // WorkflowAction Entity
        CreateMap<WorkflowAction, WorkflowActionDto>()
            .ForMember(dest => dest.HasConditions, opt => opt.MapFrom(src => src.HasConditions));

        // WorkflowScheme Aggregate
        CreateMap<WorkflowScheme, WorkflowSchemeDto>()
            .ForMember(dest => dest.Mappings, opt => opt.MapFrom(src => src.Mappings));

        // WorkflowSchemeMapping Entity
        CreateMap<WorkflowSchemeMapping, WorkflowSchemeMappingDto>();

        // WorkflowCustomField Entity
        CreateMap<WorkflowCustomField, WorkflowCustomFieldDto>();
    }
}

