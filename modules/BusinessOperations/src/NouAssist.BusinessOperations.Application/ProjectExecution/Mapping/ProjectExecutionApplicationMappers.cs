using NouAssist.BusinessOperations.ProjectExecution.Aggregates;
using NouAssist.BusinessOperations.ProjectExecution.Entities;
using NouAssist.BusinessOperations.ProjectExecution.Projects.Dtos;
using NouAssist.BusinessOperations.ProjectExecution.Tasks.Dtos;
using NouAssist.BusinessOperations.ProjectExecution.TimeLogs.Dtos;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace NouAssist.BusinessOperations.ProjectExecution.Mapping
{
    /* Project Mappers */

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ProjectToProjectDtoMapper : MapperBase<Project, ProjectDto>
    {
        [MapperIgnoreTarget(nameof(ProjectDto.CompletionPercentage))]
        [MapperIgnoreTarget(nameof(ProjectDto.TotalLoggedHours))]
        public override partial ProjectDto Map(Project source);

        public override partial void Map(Project source, ProjectDto destination);

        public override void AfterMap(Project source, ProjectDto destination)
        {
            destination.Name = source.Details.Name;
            destination.Description = source.Details.Description;
            destination.StartDate = source.Details.StartDate;
            destination.PlannedEndDate = source.Details.PlannedEndDate;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ProjectToProjectWithDetailsDtoMapper : MapperBase<Project, ProjectWithDetailsDto>
    {
        [MapperIgnoreTarget(nameof(ProjectWithDetailsDto.CompletionPercentage))]
        [MapperIgnoreTarget(nameof(ProjectWithDetailsDto.TotalLoggedHours))]
        public override partial ProjectWithDetailsDto Map(Project source);

        public override partial void Map(Project source, ProjectWithDetailsDto destination);

        public override void AfterMap(Project source, ProjectWithDetailsDto destination)
        {
            destination.Name = source.Details.Name;
            destination.Description = source.Details.Description;
            destination.StartDate = source.Details.StartDate;
            destination.PlannedEndDate = source.Details.PlannedEndDate;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ProjectPhaseToProjectPhaseDtoMapper : MapperBase<ProjectPhase, ProjectPhaseDto>
    {
        [MapperIgnoreTarget(nameof(ProjectPhaseDto.CompletionPercentage))]
        public override partial ProjectPhaseDto Map(ProjectPhase source);

        public override partial void Map(ProjectPhase source, ProjectPhaseDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ProjectMilestoneToProjectMilestoneDtoMapper : MapperBase<ProjectMilestone, ProjectMilestoneDto>
    {
        public override partial ProjectMilestoneDto Map(ProjectMilestone source);

        public override partial void Map(ProjectMilestone source, ProjectMilestoneDto destination);

        public override void AfterMap(ProjectMilestone source, ProjectMilestoneDto destination)
        {
            destination.IsCompleted = source.IsCompleted;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ProjectMemberToProjectMemberDtoMapper : MapperBase<ProjectMember, ProjectMemberDto>
    {
        [MapperIgnoreTarget(nameof(ProjectMemberDto.UserName))]
        public override partial ProjectMemberDto Map(ProjectMember source);

        public override partial void Map(ProjectMember source, ProjectMemberDto destination);

        public override void AfterMap(ProjectMember source, ProjectMemberDto destination)
        {
            destination.IsActive = source.IsActive;
        }
    }

    /* Task Mappers */

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class TaskToTaskDtoMapper : MapperBase<Task, TaskDto>
    {
        [MapperIgnoreTarget(nameof(TaskDto.TotalLoggedHours))]
        [MapperIgnoreTarget(nameof(TaskDto.AssigneeIds))]
        [MapperIgnoreTarget(nameof(TaskDto.AssigneeNames))]
        public override partial TaskDto Map(Task source);

        public override partial void Map(Task source, TaskDto destination);

        public override void AfterMap(Task source, TaskDto destination)
        {
            destination.Name = source.Details.Name;
            destination.Description = source.Details.Description;
            destination.EstimatedHours = source.Details.EstimatedHours;
            destination.Priority = source.Details.Priority;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class TaskToTaskWithDetailsDtoMapper : MapperBase<Task, TaskWithDetailsDto>
    {
        [MapperIgnoreTarget(nameof(TaskWithDetailsDto.TotalLoggedHours))]
        [MapperIgnoreTarget(nameof(TaskWithDetailsDto.AssigneeIds))]
        [MapperIgnoreTarget(nameof(TaskWithDetailsDto.AssigneeNames))]
        public override partial TaskWithDetailsDto Map(Task source);

        public override partial void Map(Task source, TaskWithDetailsDto destination);

        public override void AfterMap(Task source, TaskWithDetailsDto destination)
        {
            destination.Name = source.Details.Name;
            destination.Description = source.Details.Description;
            destination.EstimatedHours = source.Details.EstimatedHours;
            destination.Priority = source.Details.Priority;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class TaskAssignmentToTaskAssignmentDtoMapper : MapperBase<TaskAssignment, TaskAssignmentDto>
    {
        [MapperIgnoreTarget(nameof(TaskAssignmentDto.UserName))]
        public override partial TaskAssignmentDto Map(TaskAssignment source);

        public override partial void Map(TaskAssignment source, TaskAssignmentDto destination);

        public override void AfterMap(TaskAssignment source, TaskAssignmentDto destination)
        {
            destination.IsActive = source.IsActive;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class TaskDependencyToTaskDependencyDtoMapper : MapperBase<TaskDependency, TaskDependencyDto>
    {
        [MapperIgnoreTarget(nameof(TaskDependencyDto.DependsOnTaskName))]
        public override partial TaskDependencyDto Map(TaskDependency source);

        public override partial void Map(TaskDependency source, TaskDependencyDto destination);
    }

    /* TimeLog Mappers */

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class TimeLogToTimeLogDtoMapper : MapperBase<TimeLog, TimeLogDto>
    {
        [MapperIgnoreTarget(nameof(TimeLogDto.UserName))]
        [MapperIgnoreTarget(nameof(TimeLogDto.ProjectName))]
        [MapperIgnoreTarget(nameof(TimeLogDto.TaskName))]
        public override partial TimeLogDto Map(TimeLog source);

        public override partial void Map(TimeLog source, TimeLogDto destination);

        public override void AfterMap(TimeLog source, TimeLogDto destination)
        {
            destination.Hours = source.Details.Hours;
            destination.LogDate = source.Details.LogDate;
            destination.Notes = source.Details.Notes;
            destination.ResourceInstanceId = source.Details.ResourceInstanceId;
        }
    }
}
