using AutoMapper;
using NouAssist.WorkManagement.ProjectManagement.Aggregates;
using NouAssist.WorkManagement.ProjectManagement.Dependencies.Dtos;
using NouAssist.WorkManagement.ProjectManagement.Entities;
using NouAssist.WorkManagement.ProjectManagement.Events.Dtos;
using NouAssist.WorkManagement.ProjectManagement.Projects.Dtos;
using NouAssist.WorkManagement.ProjectManagement.Requirements.Dtos;
using NouAssist.WorkManagement.ProjectManagement.Sprints.Dtos;
using NouAssist.WorkManagement.ProjectManagement.SuccessCriteria.Dtos;

namespace NouAssist.WorkManagement.ProjectManagement
{
    public class ProjectsApplicationAutoMapperProfile : Profile
    {
        public ProjectsApplicationAutoMapperProfile()
        {
            // Project Mappings
            CreateMap<Project, ProjectDto>()
                // Map from Priority value object
                .ForMember(dest => dest.Importance, opt => opt.MapFrom(src => src.Priority.Importance))
                .ForMember(dest => dest.Urgency, opt => opt.MapFrom(src => src.Priority.Urgency))
                // Map from Timeline value object
                .ForMember(dest => dest.PlannedStartDate, opt => opt.MapFrom(src => src.Timeline.PlannedStartDate))
                .ForMember(dest => dest.PlannedDueDate, opt => opt.MapFrom(src => src.Timeline.PlannedDueDate))
                .ForMember(dest => dest.ActualStartDate, opt => opt.MapFrom(src => src.Timeline.ActualStartDate))
                .ForMember(dest => dest.ActualCompletedDate, opt => opt.MapFrom(src => src.Timeline.ActualCompletedDate))
                // Computed properties
                .ForMember(dest => dest.IsCompleted, opt => opt.MapFrom(src => src.IsCompleted))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.IsOverdue, opt => opt.MapFrom(src => src.IsOverdue))
                .ForMember(dest => dest.Health, opt => opt.MapFrom(src => src.Health))
                .ForMember(dest => dest.SuccessCriteriaMet, opt => opt.MapFrom(src => src.SuccessCriteriaMet))
                .ForMember(dest => dest.SuccessCriteriaCompletionRate, opt => opt.MapFrom(src => src.SuccessCriteriaCompletionRate))
                .ForMember(dest => dest.HasSuccessCriteria, opt => opt.MapFrom(src => src.HasSuccessCriteria))
                .ForMember(dest => dest.CollaboratingTeamIds, opt => opt.MapFrom(src => src.CollaboratingTeamIds));

            // Sprint Mappings
            CreateMap<ProjectSprint, ProjectSprintDto>()
                // Map from Timeline value object
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.Timeline.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.Timeline.EndDate))
                .ForMember(dest => dest.ActualStartDate, opt => opt.MapFrom(src => src.Timeline.ActualStartDate))
                .ForMember(dest => dest.ActualEndDate, opt => opt.MapFrom(src => src.Timeline.ActualEndDate))
                // Map from Metrics value object
                .ForMember(dest => dest.PlannedStoryPoints, opt => opt.MapFrom(src => src.Metrics.PlannedStoryPoints))
                .ForMember(dest => dest.CompletedStoryPoints, opt => opt.MapFrom(src => src.Metrics.CompletedStoryPoints))
                .ForMember(dest => dest.VelocityEstimate, opt => opt.MapFrom(src => src.Metrics.VelocityEstimate))
                .ForMember(dest => dest.ActualVelocity, opt => opt.MapFrom(src => src.Metrics.ActualVelocity))
                // Computed properties
                .ForMember(dest => dest.DurationInDays, opt => opt.MapFrom(src => src.Timeline.DurationInDays))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.DaysRemaining, opt => opt.MapFrom(src => src.DaysRemaining))
                .ForMember(dest => dest.CompletionRate, opt => opt.MapFrom(src => src.Metrics.CompletionRate))
                .ForMember(dest => dest.Health, opt => opt.MapFrom(src => src.Health));

            // Dependency Mappings
            CreateMap<ProjectDependency, ProjectDependencyDto>()
                .ForMember(dest => dest.IsOverdue, opt => opt.MapFrom(src => src.IsOverdue))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.IsResolved, opt => opt.MapFrom(src => src.IsResolved));

            // Success Criterion Mappings
            CreateMap<ProjectSuccessCriterion, SuccessCriterionDto>()
                .ForMember(dest => dest.Criticality, opt => opt.MapFrom(src => src.GetCriticality()))
                .ForMember(dest => dest.IsOverdue, opt => opt.MapFrom(src => src.IsOverdue))
                .ForMember(dest => dest.AchievementPercentage, opt => opt.MapFrom(src => src.AchievementPercentage))
                .ForMember(dest => dest.AttachmentIds, opt => opt.MapFrom(src => src.AttachmentIds));

            // Event Mappings
            CreateMap<ProjectEvent, ProjectEventDto>()
                .ForMember(dest => dest.IsPast, opt => opt.MapFrom(src => src.IsPast))
                .ForMember(dest => dest.IsToday, opt => opt.MapFrom(src => src.IsToday))
                .ForMember(dest => dest.IsUpcoming, opt => opt.MapFrom(src => src.IsUpcoming));

            // Requirement Mappings
            CreateMap<ProjectRequirement, ProjectRequirementDto>();
        }
    }
}