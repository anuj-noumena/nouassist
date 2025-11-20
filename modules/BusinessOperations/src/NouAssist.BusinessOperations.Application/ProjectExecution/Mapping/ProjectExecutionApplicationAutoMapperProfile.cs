using AutoMapper;
using NouAssist.BusinessOperations.ProjectExecution.Aggregates;
using NouAssist.BusinessOperations.ProjectExecution.Entities;
using NouAssist.BusinessOperations.ProjectExecution.Projects.Dtos;
using NouAssist.BusinessOperations.ProjectExecution.Tasks.Dtos;
using NouAssist.BusinessOperations.ProjectExecution.TimeLogs.Dtos;

namespace NouAssist.BusinessOperations.ProjectExecution.Mapping
{
    /// <summary>
    /// AutoMapper profile for Project Execution module
    /// </summary>
    public class ProjectExecutionApplicationAutoMapperProfile : Profile
    {
        public ProjectExecutionApplicationAutoMapperProfile()
        {
            // Project -> ProjectDto
            CreateMap<Project, ProjectDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Details.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Details.Description))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.Details.StartDate))
                .ForMember(dest => dest.PlannedEndDate, opt => opt.MapFrom(src => src.Details.PlannedEndDate))
                .ForMember(dest => dest.CompletionPercentage, opt => opt.Ignore())
                .ForMember(dest => dest.TotalLoggedHours, opt => opt.Ignore());
                
            // Project -> ProjectWithDetailsDto
            CreateMap<Project, ProjectWithDetailsDto>()
                .IncludeBase<Project, ProjectDto>();
                
            // ProjectPhase -> ProjectPhaseDto
            CreateMap<ProjectPhase, ProjectPhaseDto>()
                .ForMember(dest => dest.CompletionPercentage, opt => opt.Ignore());
                
            // ProjectMilestone -> ProjectMilestoneDto
            CreateMap<ProjectMilestone, ProjectMilestoneDto>()
                .ForMember(dest => dest.IsCompleted, opt => opt.MapFrom(src => src.IsCompleted));
                
            // ProjectMember -> ProjectMemberDto
            CreateMap<ProjectMember, ProjectMemberDto>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.UserName, opt => opt.Ignore());
                
            // Task -> TaskDto
            CreateMap<Task, TaskDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Details.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Details.Description))
                .ForMember(dest => dest.EstimatedHours, opt => opt.MapFrom(src => src.Details.EstimatedHours))
                .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Details.Priority))
                .ForMember(dest => dest.TotalLoggedHours, opt => opt.Ignore())
                .ForMember(dest => dest.AssigneeIds, opt => opt.Ignore())
                .ForMember(dest => dest.AssigneeNames, opt => opt.Ignore());
                
            // Task -> TaskWithDetailsDto
            CreateMap<Task, TaskWithDetailsDto>()
                .IncludeBase<Task, TaskDto>();
                
            // TaskAssignment -> TaskAssignmentDto
            CreateMap<TaskAssignment, TaskAssignmentDto>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.UserName, opt => opt.Ignore());
                
            // TaskDependency -> TaskDependencyDto
            CreateMap<TaskDependency, TaskDependencyDto>()
                .ForMember(dest => dest.DependsOnTaskName, opt => opt.Ignore());
                
            // TimeLog -> TimeLogDto
            CreateMap<TimeLog, TimeLogDto>()
                .ForMember(dest => dest.Hours, opt => opt.MapFrom(src => src.Details.Hours))
                .ForMember(dest => dest.LogDate, opt => opt.MapFrom(src => src.Details.LogDate))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Details.Notes))
                .ForMember(dest => dest.ResourceInstanceId, opt => opt.MapFrom(src => src.Details.ResourceInstanceId))
                .ForMember(dest => dest.UserName, opt => opt.Ignore())
                .ForMember(dest => dest.ProjectName, opt => opt.Ignore())
                .ForMember(dest => dest.TaskName, opt => opt.Ignore());
        }
    }
}
