using System.Linq;
using NouAssist.WorkManagement.ProjectManagement.Aggregates;
using NouAssist.WorkManagement.ProjectManagement.Dependencies.Dtos;
using NouAssist.WorkManagement.ProjectManagement.Entities;
using NouAssist.WorkManagement.ProjectManagement.Events.Dtos;
using NouAssist.WorkManagement.ProjectManagement.Projects.Dtos;
using NouAssist.WorkManagement.ProjectManagement.Requirements.Dtos;
using NouAssist.WorkManagement.ProjectManagement.Sprints.Dtos;
using NouAssist.WorkManagement.ProjectManagement.SuccessCriteria.Dtos;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace NouAssist.WorkManagement.ProjectManagement
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ProjectToProjectDtoMapper : MapperBase<Project, ProjectDto>
    {
        [MapperIgnoreTarget(nameof(ProjectDto.Importance))]
        [MapperIgnoreTarget(nameof(ProjectDto.Urgency))]
        [MapperIgnoreTarget(nameof(ProjectDto.PlannedStartDate))]
        [MapperIgnoreTarget(nameof(ProjectDto.PlannedDueDate))]
        [MapperIgnoreTarget(nameof(ProjectDto.ActualStartDate))]
        [MapperIgnoreTarget(nameof(ProjectDto.ActualCompletedDate))]
        public override partial ProjectDto Map(Project source);

        public override partial void Map(Project source, ProjectDto destination);

        public override void AfterMap(Project source, ProjectDto destination)
        {
            // Map from Priority value object
            destination.Importance = source.Priority.Importance;
            destination.Urgency = source.Priority.Urgency;

            // Map from Timeline value object
            destination.PlannedStartDate = source.Timeline.PlannedStartDate;
            destination.PlannedDueDate = source.Timeline.PlannedDueDate;
            destination.ActualStartDate = source.Timeline.ActualStartDate;
            destination.ActualCompletedDate = source.Timeline.ActualCompletedDate;

            // Computed properties
            destination.IsCompleted = source.IsCompleted;
            destination.IsActive = source.IsActive;
            destination.IsOverdue = source.IsOverdue;
            destination.Health = source.Health;
            destination.SuccessCriteriaMet = source.SuccessCriteriaMet;
            destination.SuccessCriteriaCompletionRate = source.SuccessCriteriaCompletionRate;
            destination.HasSuccessCriteria = source.HasSuccessCriteria;
            destination.CollaboratingTeamIds = source.CollaboratingTeamIds.ToList();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ProjectSprintToProjectSprintDtoMapper : MapperBase<ProjectSprint, ProjectSprintDto>
    {
        [MapperIgnoreTarget(nameof(ProjectSprintDto.StartDate))]
        [MapperIgnoreTarget(nameof(ProjectSprintDto.EndDate))]
        [MapperIgnoreTarget(nameof(ProjectSprintDto.ActualStartDate))]
        [MapperIgnoreTarget(nameof(ProjectSprintDto.ActualEndDate))]
        [MapperIgnoreTarget(nameof(ProjectSprintDto.PlannedStoryPoints))]
        [MapperIgnoreTarget(nameof(ProjectSprintDto.CompletedStoryPoints))]
        [MapperIgnoreTarget(nameof(ProjectSprintDto.VelocityEstimate))]
        [MapperIgnoreTarget(nameof(ProjectSprintDto.ActualVelocity))]
        [MapperIgnoreTarget(nameof(ProjectSprintDto.DurationInDays))]
        [MapperIgnoreTarget(nameof(ProjectSprintDto.CompletionRate))]
        public override partial ProjectSprintDto Map(ProjectSprint source);

        public override partial void Map(ProjectSprint source, ProjectSprintDto destination);

        public override void AfterMap(ProjectSprint source, ProjectSprintDto destination)
        {
            // Map from Timeline value object
            destination.StartDate = source.Timeline.StartDate;
            destination.EndDate = source.Timeline.EndDate;
            destination.ActualStartDate = source.Timeline.ActualStartDate;
            destination.ActualEndDate = source.Timeline.ActualEndDate;
            destination.DurationInDays = source.Timeline.DurationInDays;

            // Map from Metrics value object
            destination.PlannedStoryPoints = source.Metrics.PlannedStoryPoints;
            destination.CompletedStoryPoints = source.Metrics.CompletedStoryPoints;
            destination.VelocityEstimate = source.Metrics.VelocityEstimate;
            destination.ActualVelocity = source.Metrics.ActualVelocity;
            destination.CompletionRate = source.Metrics.CompletionRate;

            // Computed properties
            destination.IsActive = source.IsActive;
            destination.DaysRemaining = source.DaysRemaining;
            destination.Health = source.Health;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ProjectDependencyToProjectDependencyDtoMapper : MapperBase<ProjectDependency, ProjectDependencyDto>
    {
        public override partial ProjectDependencyDto Map(ProjectDependency source);

        public override partial void Map(ProjectDependency source, ProjectDependencyDto destination);

        public override void AfterMap(ProjectDependency source, ProjectDependencyDto destination)
        {
            destination.IsOverdue = source.IsOverdue;
            destination.IsActive = source.IsActive;
            destination.IsResolved = source.IsResolved;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ProjectSuccessCriterionToSuccessCriterionDtoMapper : MapperBase<ProjectSuccessCriterion, SuccessCriterionDto>
    {
        [MapperIgnoreTarget(nameof(SuccessCriterionDto.Criticality))]
        public override partial SuccessCriterionDto Map(ProjectSuccessCriterion source);

        public override partial void Map(ProjectSuccessCriterion source, SuccessCriterionDto destination);

        public override void AfterMap(ProjectSuccessCriterion source, SuccessCriterionDto destination)
        {
            destination.Criticality = source.GetCriticality();
            destination.IsOverdue = source.IsOverdue;
            destination.AchievementPercentage = source.AchievementPercentage;
            destination.AttachmentIds = source.AttachmentIds.ToList();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ProjectEventToProjectEventDtoMapper : MapperBase<ProjectEvent, ProjectEventDto>
    {
        public override partial ProjectEventDto Map(ProjectEvent source);

        public override partial void Map(ProjectEvent source, ProjectEventDto destination);

        public override void AfterMap(ProjectEvent source, ProjectEventDto destination)
        {
            destination.IsPast = source.IsPast;
            destination.IsToday = source.IsToday;
            destination.IsUpcoming = source.IsUpcoming;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ProjectRequirementToProjectRequirementDtoMapper : MapperBase<ProjectRequirement, ProjectRequirementDto>
    {
        public override partial ProjectRequirementDto Map(ProjectRequirement source);

        public override partial void Map(ProjectRequirement source, ProjectRequirementDto destination);
    }
}
