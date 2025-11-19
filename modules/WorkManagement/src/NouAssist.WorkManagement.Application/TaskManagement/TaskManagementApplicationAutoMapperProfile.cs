using System;
using AutoMapper;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using NouAssist.WorkManagement.TaskManagement.Entities;
using NouAssist.WorkManagement.TaskManagement.TaskInstances.Dtos;
using NouAssist.WorkManagement.TaskManagement.Tasks.Dtos;
using NouAssist.WorkManagement.TaskManagement.TaskTypes.Dtos;
using NouAssist.WorkManagement.TaskManagement.ValueObjects;

namespace NouAssist.WorkManagement.TaskManagement
{
    public class TasksApplicationAutoMapperProfile : Profile
    {
        public TasksApplicationAutoMapperProfile()
        {
            // Task mappings - Updated from ActionSteps to ChecklistItems
            CreateMap<Task, TaskDto>()
                .ForMember(dest => dest.ChecklistItems, opt => opt.MapFrom(src => src.ChecklistItems))
                .ForMember(dest => dest.AcceptanceCriteria, opt => opt.MapFrom(src => src.AcceptanceCriteria))
                .ForMember(dest => dest.IsInArea, opt => opt.MapFrom(src => src.IsInArea))
                .ForMember(dest => dest.IsInProject, opt => opt.MapFrom(src => src.IsInProject))
                .ForMember(dest => dest.IsInContext, opt => opt.MapFrom(src => src.IsInContext))
                .ForMember(dest => dest.IsOrganized, opt => opt.MapFrom(src => src.IsOrganized));

            CreateMap<ChecklistItem, ChecklistItemDto>();
            
            CreateMap<AcceptanceCriterion, AcceptanceCriterionDto>()
                .ForMember(dest => dest.HasGivenWhenThen, opt => opt.MapFrom(src => src.HasGivenWhenThen));

            CreateMap<RecurrenceRule, RecurrenceRuleDto>()
                .ForMember(dest => dest.Frequency, opt => opt.MapFrom(src => src.Frequency.ToString()));

            CreateMap<ComplexityEstimation, ComplexityEstimationDto>()
                .ForMember(dest => dest.DisplayValue, opt => opt.MapFrom(src => src.ToString()))
                .ForMember(dest => dest.NormalizedComplexity, opt => opt.MapFrom(src => src.GetNormalizedComplexity()));

            // TaskInstance mappings - Map from value objects
            CreateMap<TaskInstance, TaskInstanceDto>()
                // Work Tracking
                .ForMember(dest => dest.ProgressPercent, opt => opt.MapFrom(src => src.WorkTracking.ProgressPercent))
                .ForMember(dest => dest.StartedAt, opt => opt.MapFrom(src => src.WorkTracking.StartedAt))
                .ForMember(dest => dest.CompletedAt, opt => opt.MapFrom(src => src.WorkTracking.CompletedAt))
                .ForMember(dest => dest.ActualMinutes, opt => opt.MapFrom(src => src.WorkTracking.ActualMinutes))
                .ForMember(dest => dest.EstimatedMinutes, opt => opt.MapFrom(src => src.WorkTracking.EstimatedMinutes))
                .ForMember(dest => dest.IsNextAction, opt => opt.MapFrom(src => src.WorkTracking.IsNextAction))
                
                // Schedule
                .ForMember(dest => dest.PlannedStartDate, opt => opt.MapFrom(src => src.Schedule.PlannedStartDate))
                .ForMember(dest => dest.PlannedDueDate, opt => opt.MapFrom(src => src.Schedule.PlannedDueDate))
                .ForMember(dest => dest.ActualStartDate, opt => opt.MapFrom(src => src.Schedule.ActualStartDate))
                .ForMember(dest => dest.ActualDueDate, opt => opt.MapFrom(src => src.Schedule.ActualDueDate))
                
                // Priority
                .ForMember(dest => dest.Importance, opt => opt.MapFrom(src => src.Priority.Importance))
                .ForMember(dest => dest.Urgency, opt => opt.MapFrom(src => src.Priority.Urgency))
                .ForMember(dest => dest.EisenhowerQuadrant, opt => opt.MapFrom(src => src.Priority.Quadrant.ToString()))
                
                // Hierarchy - Not yet implemented in TaskInstance
                .ForMember(dest => dest.ParentInstanceId, opt => opt.MapFrom<Guid?>(src => null))
                .ForMember(dest => dest.HierarchyLevel, opt => opt.MapFrom<int>(src => 0))
                
                // Collaboration
                .ForMember(dest => dest.CommentsCount, opt => opt.MapFrom(src => src.Collaboration.CommentCount))
                .ForMember(dest => dest.AttachmentsCount, opt => opt.MapFrom(src => src.Collaboration.AttachmentCount))
                .ForMember(dest => dest.Watchers, opt => opt.MapFrom(src => src.Collaboration.Watchers))
                
                // Computed properties
                .ForMember(dest => dest.IsOverdue, opt => opt.MapFrom(src => src.IsOverdue))
                .ForMember(dest => dest.IsCompleted, opt => opt.MapFrom(src => src.WorkTracking.IsCompleted))
                .ForMember(dest => dest.HasStarted, opt => opt.MapFrom(src => src.WorkTracking.HasStarted))
                .ForMember(dest => dest.IsDelegated, opt => opt.MapFrom<bool>(src => false)) // DelegationInfo not yet implemented
                .ForMember(dest => dest.DelegationInfo, opt => opt.MapFrom<DelegationInfoDto>(src => null)) // DelegationInfo not yet implemented
                .ForMember(dest => dest.HasBlockingDependencies, opt => opt.MapFrom(src => src.HasBlockingDependencies))
                .ForMember(dest => dest.TimeRemaining, opt => opt.MapFrom(src => src.Schedule.TimeRemaining))
                .ForMember(dest => dest.IsResolved, opt => opt.MapFrom(src => src.IsResolved))
                
                // GTD Computed properties
                .ForMember(dest => dest.IsInArea, opt => opt.MapFrom(src => src.IsInArea))
                .ForMember(dest => dest.IsInProject, opt => opt.MapFrom(src => src.IsInProject))
                .ForMember(dest => dest.IsInContext, opt => opt.MapFrom(src => src.IsInContext))
                .ForMember(dest => dest.IsOrganized, opt => opt.MapFrom(src => src.IsOrganized))
                .ForMember(dest => dest.IsAreaTask, opt => opt.MapFrom(src => src.IsAreaTask))
                .ForMember(dest => dest.IsProjectTask, opt => opt.MapFrom(src => src.IsProjectTask));

            CreateMap<TaskInstance, TaskInstanceDetailDto>()
                .IncludeBase<TaskInstance, TaskInstanceDto>()
                .ForMember(dest => dest.ChecklistItemCompletions, opt => opt.MapFrom(src => src.ChecklistItems))
                .ForMember(dest => dest.CriterionCompletions, opt => opt.MapFrom(src => src.CriterionCompletions))
                .ForMember(dest => dest.Dependencies, opt => opt.MapFrom(src => src.Dependencies));

            CreateMap<DelegationInfo, DelegationInfoDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<TaskChecklistItem, ChecklistItemCompletionDto>();
            CreateMap<AcceptanceCriterionCompletion, AcceptanceCriterionCompletionDto>();
            CreateMap<TaskDependency, TaskDependencyDto>()
                .ForMember(dest => dest.DependencyType, opt => opt.MapFrom(src => src.DependencyType.ToString()));

            // TaskType mappings
            CreateMap<TaskType, TaskTypeDto>()
                .ForMember(dest => dest.CustomFields, opt => opt.MapFrom(src => src.CustomFields));

            CreateMap<TaskTypeCustomField, TaskTypeCustomFieldDto>();
        }
    }
}