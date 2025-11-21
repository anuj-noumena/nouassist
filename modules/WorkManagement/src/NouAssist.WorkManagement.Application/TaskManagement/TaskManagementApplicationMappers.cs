using System;
using System.Linq;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using NouAssist.WorkManagement.TaskManagement.Entities;
using NouAssist.WorkManagement.TaskManagement.TaskInstances.Dtos;
using NouAssist.WorkManagement.TaskManagement.Tasks.Dtos;
using NouAssist.WorkManagement.TaskManagement.TaskTypes.Dtos;
using NouAssist.WorkManagement.TaskManagement.ValueObjects;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace NouAssist.WorkManagement.TaskManagement
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class TaskToTaskDtoMapper : MapperBase<Task, TaskDto>
    {
        public override partial TaskDto Map(Task source);

        public override partial void Map(Task source, TaskDto destination);

        public override void AfterMap(Task source, TaskDto destination)
        {
            var checklistMapper = new ChecklistItemToChecklistItemDtoMapper();
            destination.ChecklistItems = source.ChecklistItems.Select(checklistMapper.Map).ToList();

            var criterionMapper = new AcceptanceCriterionToAcceptanceCriterionDtoMapper();
            destination.AcceptanceCriteria = source.AcceptanceCriteria.Select(criterionMapper.Map).ToList();

            destination.IsInArea = source.IsInArea;
            destination.IsInProject = source.IsInProject;
            destination.IsInContext = source.IsInContext;
            destination.IsOrganized = source.IsOrganized;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ChecklistItemToChecklistItemDtoMapper : MapperBase<ChecklistItem, ChecklistItemDto>
    {
        public override partial ChecklistItemDto Map(ChecklistItem source);

        public override partial void Map(ChecklistItem source, ChecklistItemDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class AcceptanceCriterionToAcceptanceCriterionDtoMapper : MapperBase<AcceptanceCriterion, AcceptanceCriterionDto>
    {
        public override partial AcceptanceCriterionDto Map(AcceptanceCriterion source);

        public override partial void Map(AcceptanceCriterion source, AcceptanceCriterionDto destination);

        public override void AfterMap(AcceptanceCriterion source, AcceptanceCriterionDto destination)
        {
            destination.HasGivenWhenThen = source.HasGivenWhenThen;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class RecurrenceRuleToRecurrenceRuleDtoMapper : MapperBase<RecurrenceRule, RecurrenceRuleDto>
    {
        [MapperIgnoreTarget(nameof(RecurrenceRuleDto.Frequency))]
        public override partial RecurrenceRuleDto Map(RecurrenceRule source);

        public override partial void Map(RecurrenceRule source, RecurrenceRuleDto destination);

        public override void AfterMap(RecurrenceRule source, RecurrenceRuleDto destination)
        {
            destination.Frequency = source.Frequency.ToString();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ComplexityEstimationToComplexityEstimationDtoMapper : MapperBase<ComplexityEstimation, ComplexityEstimationDto>
    {
        [MapperIgnoreTarget(nameof(ComplexityEstimationDto.DisplayValue))]
        [MapperIgnoreTarget(nameof(ComplexityEstimationDto.NormalizedComplexity))]
        public override partial ComplexityEstimationDto Map(ComplexityEstimation source);

        public override partial void Map(ComplexityEstimation source, ComplexityEstimationDto destination);

        public override void AfterMap(ComplexityEstimation source, ComplexityEstimationDto destination)
        {
            destination.DisplayValue = source.ToString();
            destination.NormalizedComplexity = source.GetNormalizedComplexity();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class TaskInstanceToTaskInstanceDtoMapper : MapperBase<TaskInstance, TaskInstanceDto>
    {
        [MapperIgnoreTarget(nameof(TaskInstanceDto.ProgressPercent))]
        [MapperIgnoreTarget(nameof(TaskInstanceDto.StartedAt))]
        [MapperIgnoreTarget(nameof(TaskInstanceDto.CompletedAt))]
        [MapperIgnoreTarget(nameof(TaskInstanceDto.ActualMinutes))]
        [MapperIgnoreTarget(nameof(TaskInstanceDto.EstimatedMinutes))]
        [MapperIgnoreTarget(nameof(TaskInstanceDto.IsNextAction))]
        [MapperIgnoreTarget(nameof(TaskInstanceDto.PlannedStartDate))]
        [MapperIgnoreTarget(nameof(TaskInstanceDto.PlannedDueDate))]
        [MapperIgnoreTarget(nameof(TaskInstanceDto.ActualStartDate))]
        [MapperIgnoreTarget(nameof(TaskInstanceDto.ActualDueDate))]
        [MapperIgnoreTarget(nameof(TaskInstanceDto.Importance))]
        [MapperIgnoreTarget(nameof(TaskInstanceDto.Urgency))]
        [MapperIgnoreTarget(nameof(TaskInstanceDto.EisenhowerQuadrant))]
        [MapperIgnoreTarget(nameof(TaskInstanceDto.ParentInstanceId))]
        [MapperIgnoreTarget(nameof(TaskInstanceDto.HierarchyLevel))]
        [MapperIgnoreTarget(nameof(TaskInstanceDto.CommentsCount))]
        [MapperIgnoreTarget(nameof(TaskInstanceDto.AttachmentsCount))]
        [MapperIgnoreTarget(nameof(TaskInstanceDto.Watchers))]
        [MapperIgnoreTarget(nameof(TaskInstanceDto.IsDelegated))]
        [MapperIgnoreTarget(nameof(TaskInstanceDto.DelegationInfo))]
        public override partial TaskInstanceDto Map(TaskInstance source);

        public override partial void Map(TaskInstance source, TaskInstanceDto destination);

        public override void AfterMap(TaskInstance source, TaskInstanceDto destination)
        {
            // Work Tracking
            destination.ProgressPercent = source.WorkTracking.ProgressPercent;
            destination.StartedAt = source.WorkTracking.StartedAt;
            destination.CompletedAt = source.WorkTracking.CompletedAt;
            destination.ActualMinutes = source.WorkTracking.ActualMinutes;
            destination.EstimatedMinutes = source.WorkTracking.EstimatedMinutes;
            destination.IsNextAction = source.WorkTracking.IsNextAction;

            // Schedule
            destination.PlannedStartDate = source.Schedule.PlannedStartDate;
            destination.PlannedDueDate = source.Schedule.PlannedDueDate;
            destination.ActualStartDate = source.Schedule.ActualStartDate;
            destination.ActualDueDate = source.Schedule.ActualDueDate;

            // Priority
            destination.Importance = source.Priority.Importance;
            destination.Urgency = source.Priority.Urgency;
            destination.EisenhowerQuadrant = source.Priority.Quadrant.ToString();

            // Hierarchy - Not yet implemented in TaskInstance
            destination.ParentInstanceId = null;
            destination.HierarchyLevel = 0;

            // Collaboration
            destination.CommentsCount = source.Collaboration.CommentCount;
            destination.AttachmentsCount = source.Collaboration.AttachmentCount;
            destination.Watchers = source.Collaboration.Watchers.ToList();

            // Computed properties
            destination.IsOverdue = source.IsOverdue;
            destination.IsCompleted = source.WorkTracking.IsCompleted;
            destination.HasStarted = source.WorkTracking.HasStarted;
            destination.IsDelegated = false; // DelegationInfo not yet implemented
            destination.DelegationInfo = null; // DelegationInfo not yet implemented
            destination.HasBlockingDependencies = source.HasBlockingDependencies;
            destination.TimeRemaining = source.Schedule.TimeRemaining;
            destination.IsResolved = source.IsResolved;

            // GTD Computed properties
            destination.IsInArea = source.IsInArea;
            destination.IsInProject = source.IsInProject;
            destination.IsInContext = source.IsInContext;
            destination.IsOrganized = source.IsOrganized;
            destination.IsAreaTask = source.IsAreaTask;
            destination.IsProjectTask = source.IsProjectTask;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class TaskInstanceToTaskInstanceDetailDtoMapper : MapperBase<TaskInstance, TaskInstanceDetailDto>
    {
        private readonly TaskInstanceToTaskInstanceDtoMapper _baseMapper;

        public TaskInstanceToTaskInstanceDetailDtoMapper(TaskInstanceToTaskInstanceDtoMapper baseMapper)
        {
            _baseMapper = baseMapper;
        }

        public override partial TaskInstanceDetailDto Map(TaskInstance source);

        public override partial void Map(TaskInstance source, TaskInstanceDetailDto destination);

        public override void AfterMap(TaskInstance source, TaskInstanceDetailDto destination)
        {
            // Apply base mapping logic
            var baseDto = _baseMapper.Map(source);
            _baseMapper.Map(source, destination);

            var checklistMapper = new TaskChecklistItemToChecklistItemCompletionDtoMapper();
            destination.ChecklistItemCompletions = source.ChecklistItems.Select(checklistMapper.Map).ToList();

            var criterionMapper = new AcceptanceCriterionCompletionToAcceptanceCriterionCompletionDtoMapper();
            destination.CriterionCompletions = source.CriterionCompletions.Select(criterionMapper.Map).ToList();

            var dependencyMapper = new TaskDependencyToTaskDependencyDtoMapper();
            destination.Dependencies = source.Dependencies.Select(dependencyMapper.Map).ToList();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class DelegationInfoToDelegationInfoDtoMapper : MapperBase<DelegationInfo, DelegationInfoDto>
    {
        [MapperIgnoreTarget(nameof(DelegationInfoDto.Status))]
        public override partial DelegationInfoDto Map(DelegationInfo source);

        public override partial void Map(DelegationInfo source, DelegationInfoDto destination);

        public override void AfterMap(DelegationInfo source, DelegationInfoDto destination)
        {
            destination.Status = source.Status.ToString();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class TaskChecklistItemToChecklistItemCompletionDtoMapper : MapperBase<TaskChecklistItem, ChecklistItemCompletionDto>
    {
        public override partial ChecklistItemCompletionDto Map(TaskChecklistItem source);

        public override partial void Map(TaskChecklistItem source, ChecklistItemCompletionDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class AcceptanceCriterionCompletionToAcceptanceCriterionCompletionDtoMapper : MapperBase<AcceptanceCriterionCompletion, AcceptanceCriterionCompletionDto>
    {
        public override partial AcceptanceCriterionCompletionDto Map(AcceptanceCriterionCompletion source);

        public override partial void Map(AcceptanceCriterionCompletion source, AcceptanceCriterionCompletionDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class TaskDependencyToTaskDependencyDtoMapper : MapperBase<TaskDependency, TaskDependencyDto>
    {
        [MapperIgnoreTarget(nameof(TaskDependencyDto.DependencyType))]
        public override partial TaskDependencyDto Map(TaskDependency source);

        public override partial void Map(TaskDependency source, TaskDependencyDto destination);

        public override void AfterMap(TaskDependency source, TaskDependencyDto destination)
        {
            destination.DependencyType = source.DependencyType.ToString();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class TaskTypeToTaskTypeDtoMapper : MapperBase<TaskType, TaskTypeDto>
    {
        public override partial TaskTypeDto Map(TaskType source);

        public override partial void Map(TaskType source, TaskTypeDto destination);

        public override void AfterMap(TaskType source, TaskTypeDto destination)
        {
            var customFieldMapper = new TaskTypeCustomFieldToTaskTypeCustomFieldDtoMapper();
            destination.CustomFields = source.CustomFields.Select(customFieldMapper.Map).ToList();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class TaskTypeCustomFieldToTaskTypeCustomFieldDtoMapper : MapperBase<TaskTypeCustomField, TaskTypeCustomFieldDto>
    {
        public override partial TaskTypeCustomFieldDto Map(TaskTypeCustomField source);

        public override partial void Map(TaskTypeCustomField source, TaskTypeCustomFieldDto destination);
    }
}
