using AutoMapper;
using NouAssist.WorkManagement.Workflow.Aggregates;
using NouAssist.WorkManagement.Workflow.Dtos;
using NouAssist.WorkManagement.Workflow.Entities;
using System.Text.Json;

namespace NouAssist.WorkManagement.Workflow;

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

        // WorkflowTransition Entity - UPDATED
        CreateMap<WorkflowTransition, WorkflowTransitionDto>()
            .ForMember(dest => dest.Purpose, opt => opt.MapFrom(src => src.Purpose))
            .ForMember(dest => dest.HasConditions, opt => opt.MapFrom(src => src.HasConditions))
            .ForMember(dest => dest.HasValidators, opt => opt.MapFrom(src => src.HasValidators))
            .ForMember(dest => dest.Actions, opt => opt.MapFrom(src => src.Actions));
        // Note: Removed AllowedRoles, AllowedUserIds, HasRestrictions mappings

        // WorkflowAction Entity
        CreateMap<WorkflowAction, WorkflowActionDto>()
            .ForMember(dest => dest.HasConditions, opt => opt.MapFrom(src => src.HasConditions));

        // WorkflowScheme Aggregate
        CreateMap<WorkflowScheme, WorkflowSchemeDto>()
            .ForMember(dest => dest.Mappings, opt => opt.MapFrom(src => src.Mappings));

        // WorkflowSchemeMapping Entity
        CreateMap<WorkflowSchemeMapping, WorkflowSchemeMappingDto>();

        // WorkflowCustomField Entity - UPDATED with custom mapping
        CreateMap<WorkflowCustomField, WorkflowCustomFieldDto>()
            .ForMember(dest => dest.VisibilityRules, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.VisibilityRules)
                    ? new FieldVisibilityRulesDto()
                    : JsonSerializer.Deserialize<FieldVisibilityRulesDto>(src.VisibilityRules)))
            .ForMember(dest => dest.ValidationRules, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.ValidationRules)
                    ? new FieldValidationRulesDto()
                    : JsonSerializer.Deserialize<FieldValidationRulesDto>(src.ValidationRules)));

        // DTO to Entity mappings for WorkflowCustomField
        CreateMap<FieldVisibilityRulesDto, FieldVisibilityRules>();
        CreateMap<FieldVisibilityRules, FieldVisibilityRulesDto>();

        CreateMap<FieldValidationRulesDto, FieldValidationRules>();
        CreateMap<FieldValidationRules, FieldValidationRulesDto>();

        // TransitionPermissionPolicy Aggregate
        CreateMap<TransitionPermissionPolicy, TransitionPermissionPolicyDto>()
            .ForMember(dest => dest.HasRestrictions, opt => opt.MapFrom(src => src.HasRestrictions))
            .ForMember(dest => dest.AllowedRoles, opt => opt.MapFrom(src => src.AllowedRoles))
            .ForMember(dest => dest.AllowedUserIds, opt => opt.MapFrom(src => src.AllowedUserIds));

        CreateMap<CreateWorkflowStatusDto, WorkflowStatus>();
        CreateMap<CreateWorkflowTransitionDto, WorkflowTransition>();
        CreateMap<CreateWorkflowActionDto, WorkflowAction>();
    }
}