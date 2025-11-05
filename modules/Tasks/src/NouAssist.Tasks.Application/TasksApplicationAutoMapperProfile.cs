using AutoMapper;
using NouAssist.Tasks.Aggregates;
using NouAssist.Tasks.Entities;
using NouAssist.Tasks.TaskInstances.Dtos;
using NouAssist.Tasks.Tasks.Dtos;
using NouAssist.Tasks.TaskTypes.Dtos;
using NouAssist.Tasks.ValueObjects;

namespace NouAssist.Tasks
{
    public class TasksApplicationAutoMapperProfile : Profile
    {
        public TasksApplicationAutoMapperProfile()
        {
            // Task mappings
            CreateMap<Task, TaskDto>()
                .ForMember(dest => dest.ActionSteps, opt => opt.MapFrom(src => src.ActionSteps))
                .ForMember(dest => dest.AcceptanceCriteria, opt => opt.MapFrom(src => src.AcceptanceCriteria));

            CreateMap<TaskActionStep, TaskActionStepDto>();
            
            CreateMap<AcceptanceCriterion, AcceptanceCriterionDto>()
                .ForMember(dest => dest.HasGivenWhenThen, opt => opt.MapFrom(src => src.HasGivenWhenThen));

            CreateMap<RecurrenceRule, RecurrenceRuleDto>()
                .ForMember(dest => dest.Frequency, opt => opt.MapFrom(src => src.Frequency.ToString()));

            // TaskInstance mappings
            CreateMap<TaskInstance, TaskInstanceDto>()
                .ForMember(dest => dest.Importance, opt => opt.MapFrom(src => src.PriorityMatrix.Importance))
                .ForMember(dest => dest.Urgency, opt => opt.MapFrom(src => src.PriorityMatrix.Urgency))
                .ForMember(dest => dest.EisenhowerQuadrant, opt => opt.MapFrom(src => src.PriorityMatrix.Quadrant.ToString()))
                .ForMember(dest => dest.TaskStatus, opt => opt.MapFrom(src => src.TaskStatus.ToString()))
                .ForMember(dest => dest.IsOverdue, opt => opt.MapFrom(src => src.IsOverdue))
                .ForMember(dest => dest.IsCompleted, opt => opt.MapFrom(src => src.IsCompleted))
                .ForMember(dest => dest.IsCancelled, opt => opt.MapFrom(src => src.IsCancelled))
                .ForMember(dest => dest.HasStarted, opt => opt.MapFrom(src => src.HasStarted))
                .ForMember(dest => dest.IsDelegated, opt => opt.MapFrom(src => src.IsDelegated))
                .ForMember(dest => dest.SubtaskCompletionRate, opt => opt.MapFrom(src => src.SubtaskCompletionRate));

            CreateMap<TaskInstance, TaskInstanceDetailDto>()
                .IncludeBase<TaskInstance, TaskInstanceDto>()
                .ForMember(dest => dest.StepCompletions, opt => opt.MapFrom(src => src.StepCompletions))
                .ForMember(dest => dest.CriterionCompletions, opt => opt.MapFrom(src => src.CriterionCompletions))
                .ForMember(dest => dest.Dependencies, opt => opt.MapFrom(src => src.Dependencies));

            CreateMap<DelegationInfo, DelegationInfoDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<TaskActionStepCompletion, TaskActionStepCompletionDto>();
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
