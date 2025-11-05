using AutoMapper;
using NouAssist.Projects.Aggregates;
using NouAssist.Projects.Dependencies.Dtos;
using NouAssist.Projects.Entities;
using NouAssist.Projects.Events.Dtos;
using NouAssist.Projects.Projects.Dtos;
using NouAssist.Projects.Requirements.Dtos;
using NouAssist.Projects.Sprints.Dtos;
using NouAssist.Projects.SuccessCriteria.Dtos;
using NouAssist.Workflow.Entities;

namespace NouAssist.Projects
{
    public class ProjectsApplicationAutoMapperProfile : Profile
    {
        public ProjectsApplicationAutoMapperProfile()
        {
            // Project Mappings
            CreateMap<Project, ProjectDto>()
                .ForMember(dest => dest.IsCompleted, opt => opt.MapFrom(src => src.IsCompleted))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.IsOverdue, opt => opt.MapFrom(src => src.IsOverdue))
                .ForMember(dest => dest.DaysRemaining, opt => opt.MapFrom(src => src.DaysRemaining))
                .ForMember(dest => dest.Health, opt => opt.MapFrom(src => src.Health))
                .ForMember(dest => dest.SuccessCriteriaCompletionRate,
                    opt => opt.MapFrom(src => src.SuccessCriteriaCompletionRate))
                .ForMember(dest => dest.CollaboratingTeamIds, opt => opt.MapFrom(src => src.CollaboratingTeamIds))
                .ForMember(dest => dest.SuccessCriterionIds, opt => opt.MapFrom(src => src.SuccessCriterionIds));

            // Sprint Mappings
            CreateMap<ProjectSprint, ProjectSprintDto>()
                .ForMember(dest => dest.DurationInDays, opt => opt.MapFrom(src => src.DurationInDays))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.DaysRemaining, opt => opt.MapFrom(src => src.DaysRemaining))
                .ForMember(dest => dest.CompletionRate, opt => opt.MapFrom(src => src.CompletionRate))
                .ForMember(dest => dest.Health, opt => opt.MapFrom(src => src.Health));

            // Dependency Mappings
            CreateMap<ProjectDependency, ProjectDependencyDto>()
                .ForMember(dest => dest.IsOverdue, opt => opt.MapFrom(src => src.IsOverdue))
                .ForMember(dest => dest.DependencyTypeDescription,
                    opt => opt.MapFrom(src => src.DependencyTypeDescription));

            // Success Criterion Mappings
            CreateMap<ProjectSuccessCriterion, SuccessCriterionDto>()
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