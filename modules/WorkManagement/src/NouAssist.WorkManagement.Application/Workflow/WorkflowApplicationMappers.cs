using System.Linq;
using NouAssist.WorkManagement.Workflow.Aggregates;
using NouAssist.WorkManagement.Workflow.Dtos;
using NouAssist.WorkManagement.Workflow.Entities;
using Riok.Mapperly.Abstractions;
using System.Text.Json;
using Volo.Abp.Mapperly;

namespace NouAssist.WorkManagement.Workflow
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class WorkflowToWorkflowDtoMapper : MapperBase<Aggregates.Workflow, WorkflowDto>
    {
        public override partial WorkflowDto Map(Aggregates.Workflow source);

        public override partial void Map(Aggregates.Workflow source, WorkflowDto destination);

        public override void AfterMap(Aggregates.Workflow source, WorkflowDto destination)
        {
            var statusMapper = new WorkflowStatusToWorkflowStatusDtoMapper();
            destination.Statuses = source.Statuses.Select(statusMapper.Map).ToList();

            var transitionMapper = new WorkflowTransitionToWorkflowTransitionDtoMapper();
            destination.Transitions = source.Transitions.Select(transitionMapper.Map).ToList();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class WorkflowStatusToWorkflowStatusDtoMapper : MapperBase<WorkflowStatus, WorkflowStatusDto>
    {
        public override partial WorkflowStatusDto Map(WorkflowStatus source);

        public override partial void Map(WorkflowStatus source, WorkflowStatusDto destination);

        public override void AfterMap(WorkflowStatus source, WorkflowStatusDto destination)
        {
            destination.IsTodo = source.IsTodo;
            destination.IsInProgress = source.IsInProgress;
            destination.IsDone = source.IsDone;
            destination.IsCancelled = source.IsCancelled;
            destination.IsBlocked = source.IsBlocked;
            destination.IsTerminal = source.IsTerminal;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class WorkflowTransitionToWorkflowTransitionDtoMapper : MapperBase<WorkflowTransition, WorkflowTransitionDto>
    {
        public override partial WorkflowTransitionDto Map(WorkflowTransition source);

        public override partial void Map(WorkflowTransition source, WorkflowTransitionDto destination);

        public override void AfterMap(WorkflowTransition source, WorkflowTransitionDto destination)
        {
            destination.Purpose = source.Purpose;
            destination.HasConditions = source.HasConditions;
            destination.HasValidators = source.HasValidators;

            var actionMapper = new WorkflowActionToWorkflowActionDtoMapper();
            destination.Actions = source.Actions.Select(actionMapper.Map).ToList();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class WorkflowActionToWorkflowActionDtoMapper : MapperBase<WorkflowAction, WorkflowActionDto>
    {
        public override partial WorkflowActionDto Map(WorkflowAction source);

        public override partial void Map(WorkflowAction source, WorkflowActionDto destination);

        public override void AfterMap(WorkflowAction source, WorkflowActionDto destination)
        {
            destination.HasConditions = source.HasConditions;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class WorkflowSchemeToWorkflowSchemeDtoMapper : MapperBase<WorkflowScheme, WorkflowSchemeDto>
    {
        public override partial WorkflowSchemeDto Map(WorkflowScheme source);

        public override partial void Map(WorkflowScheme source, WorkflowSchemeDto destination);

        public override void AfterMap(WorkflowScheme source, WorkflowSchemeDto destination)
        {
            var mappingMapper = new WorkflowSchemeMappingToWorkflowSchemeMappingDtoMapper();
            destination.Mappings = source.Mappings.Select(mappingMapper.Map).ToList();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class WorkflowSchemeMappingToWorkflowSchemeMappingDtoMapper : MapperBase<WorkflowSchemeMapping, WorkflowSchemeMappingDto>
    {
        public override partial WorkflowSchemeMappingDto Map(WorkflowSchemeMapping source);

        public override partial void Map(WorkflowSchemeMapping source, WorkflowSchemeMappingDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class WorkflowCustomFieldToWorkflowCustomFieldDtoMapper : MapperBase<WorkflowCustomField, WorkflowCustomFieldDto>
    {
        [MapperIgnoreTarget(nameof(WorkflowCustomFieldDto.VisibilityRules))]
        [MapperIgnoreTarget(nameof(WorkflowCustomFieldDto.ValidationRules))]
        public override partial WorkflowCustomFieldDto Map(WorkflowCustomField source);

        public override partial void Map(WorkflowCustomField source, WorkflowCustomFieldDto destination);

        public override void AfterMap(WorkflowCustomField source, WorkflowCustomFieldDto destination)
        {
            destination.VisibilityRules = string.IsNullOrEmpty(source.VisibilityRules)
                ? new FieldVisibilityRulesDto()
                : JsonSerializer.Deserialize<FieldVisibilityRulesDto>(source.VisibilityRules);

            destination.ValidationRules = string.IsNullOrEmpty(source.ValidationRules)
                ? new FieldValidationRulesDto()
                : JsonSerializer.Deserialize<FieldValidationRulesDto>(source.ValidationRules);
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class FieldVisibilityRulesToFieldVisibilityRulesDtoMapper : TwoWayMapperBase<FieldVisibilityRules, FieldVisibilityRulesDto>
    {
        public override partial FieldVisibilityRulesDto Map(FieldVisibilityRules source);

        public override partial void Map(FieldVisibilityRules source, FieldVisibilityRulesDto destination);

        public override partial FieldVisibilityRules ReverseMap(FieldVisibilityRulesDto source);

        public override partial void ReverseMap(FieldVisibilityRulesDto source, FieldVisibilityRules destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class FieldValidationRulesToFieldValidationRulesDtoMapper : TwoWayMapperBase<FieldValidationRules, FieldValidationRulesDto>
    {
        public override partial FieldValidationRulesDto Map(FieldValidationRules source);

        public override partial void Map(FieldValidationRules source, FieldValidationRulesDto destination);

        public override partial FieldValidationRules ReverseMap(FieldValidationRulesDto source);

        public override partial void ReverseMap(FieldValidationRulesDto source, FieldValidationRules destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class TransitionPermissionPolicyToTransitionPermissionPolicyDtoMapper : MapperBase<TransitionPermissionPolicy, TransitionPermissionPolicyDto>
    {
        public override partial TransitionPermissionPolicyDto Map(TransitionPermissionPolicy source);

        public override partial void Map(TransitionPermissionPolicy source, TransitionPermissionPolicyDto destination);

        public override void AfterMap(TransitionPermissionPolicy source, TransitionPermissionPolicyDto destination)
        {
            destination.HasRestrictions = source.HasRestrictions;
            destination.AllowedRoles = source.AllowedRoles.ToList();
            destination.AllowedUserIds = source.AllowedUserIds.Select(u => u.Value).ToList();
        }
    }
}
