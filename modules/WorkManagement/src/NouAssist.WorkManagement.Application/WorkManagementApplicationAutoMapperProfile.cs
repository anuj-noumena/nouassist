using AutoMapper;
using NouAssist.WorkManagement.TaskManagement.Aggregates.TaskInstance;
using NouAssist.WorkManagement.TaskManagement.TaskInstances.Dtos;

namespace NouAssist.WorkManagement;

public class WorkManagementApplicationAutoMapperProfile : Profile
{
    public WorkManagementApplicationAutoMapperProfile()
    {
        // TaskInstance to TaskInstanceDto mapping - only map core properties
        // Properties from split aggregates will be mapped separately in the application service
        CreateMap<TaskInstance, TaskInstanceDto>()
            // Core properties that exist on TaskInstance
            .ForMember(dest => dest.TaskId, opt => opt.MapFrom(src => src.TaskId))
            .ForMember(dest => dest.WorkflowId, opt => opt.MapFrom(src => src.WorkflowId))
            .ForMember(dest => dest.WorkflowStatusId, opt => opt.MapFrom(src => src.WorkflowStatusId))
            .ForMember(dest => dest.EntityVersion, opt => opt.MapFrom(src => src.EntityVersion))
            .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.OwnerId))
            .ForMember(dest => dest.AssigneeId, opt => opt.MapFrom(src => src.AssigneeId))
            .ForMember(dest => dest.ReporterId, opt => opt.MapFrom(src => src.ReporterId))
            .ForMember(dest => dest.TeamId, opt => opt.Ignore()) // TeamId might not be in core aggregate
            .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
            .ForMember(dest => dest.Resolution, opt => opt.MapFrom(src => src.Resolution))
            .ForMember(dest => dest.ResolutionNote, opt => opt.MapFrom(src => src.ResolutionNote))
            .ForMember(dest => dest.IsResolved, opt => opt.MapFrom(src => src.IsResolved))
            // Properties from TaskOrganizationAggregate - ignore for now
            .ForMember(dest => dest.ProjectId, opt => opt.Ignore())
            .ForMember(dest => dest.AreaId, opt => opt.Ignore())
            .ForMember(dest => dest.ContextId, opt => opt.Ignore())
            // Properties from TaskInstanceWorkAggregate - ignore for now
            .ForMember(dest => dest.ProgressPercent, opt => opt.Ignore())
            .ForMember(dest => dest.StartedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CompletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ActualMinutes, opt => opt.Ignore())
            .ForMember(dest => dest.EstimatedMinutes, opt => opt.Ignore())
            .ForMember(dest => dest.IsNextAction, opt => opt.Ignore())
            .ForMember(dest => dest.PlannedStartDate, opt => opt.Ignore())
            .ForMember(dest => dest.PlannedDueDate, opt => opt.Ignore())
            .ForMember(dest => dest.ActualStartDate, opt => opt.Ignore())
            .ForMember(dest => dest.ActualDueDate, opt => opt.Ignore())
            .ForMember(dest => dest.Importance, opt => opt.Ignore())
            .ForMember(dest => dest.Urgency, opt => opt.Ignore())
            .ForMember(dest => dest.EisenhowerQuadrant, opt => opt.Ignore())
            .ForMember(dest => dest.ComplexityEstimation, opt => opt.Ignore())
            .ForMember(dest => dest.CognitiveLoad, opt => opt.Ignore())
            // Properties from TaskHierarchyAggregate - ignore for now
            .ForMember(dest => dest.ParentInstanceId, opt => opt.Ignore())
            .ForMember(dest => dest.HierarchyLevel, opt => opt.Ignore())
            // Properties from TaskCollaborationAggregate - ignore for now
            .ForMember(dest => dest.CommentsCount, opt => opt.Ignore())
            .ForMember(dest => dest.AttachmentsCount, opt => opt.Ignore())
            .ForMember(dest => dest.Watchers, opt => opt.Ignore())
            // Properties from TaskFlowControlAggregate - ignore for now
            .ForMember(dest => dest.IsBlocked, opt => opt.Ignore())
            .ForMember(dest => dest.BlockedReason, opt => opt.Ignore())
            .ForMember(dest => dest.BlockedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsFlagged, opt => opt.Ignore())
            .ForMember(dest => dest.FlaggedReason, opt => opt.Ignore())
            .ForMember(dest => dest.RequiresApproval, opt => opt.Ignore())
            .ForMember(dest => dest.ApprovalRequestId, opt => opt.Ignore())
            .ForMember(dest => dest.DelegationInfo, opt => opt.Ignore())
            // Computed properties - ignore for now, should be set by application service
            .ForMember(dest => dest.IsOverdue, opt => opt.Ignore())
            .ForMember(dest => dest.IsCompleted, opt => opt.Ignore())
            .ForMember(dest => dest.HasStarted, opt => opt.Ignore())
            .ForMember(dest => dest.IsDelegated, opt => opt.Ignore())
            .ForMember(dest => dest.HasBlockingDependencies, opt => opt.Ignore())
            .ForMember(dest => dest.TimeRemaining, opt => opt.Ignore())
            .ForMember(dest => dest.IsInArea, opt => opt.Ignore())
            .ForMember(dest => dest.IsInProject, opt => opt.Ignore())
            .ForMember(dest => dest.IsInContext, opt => opt.Ignore())
            .ForMember(dest => dest.IsOrganized, opt => opt.Ignore())
            .ForMember(dest => dest.IsAreaTask, opt => opt.Ignore())
            .ForMember(dest => dest.IsProjectTask, opt => opt.Ignore());
    }
}
