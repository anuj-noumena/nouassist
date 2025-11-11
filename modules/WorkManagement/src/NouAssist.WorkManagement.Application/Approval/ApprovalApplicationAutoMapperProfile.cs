using AutoMapper;
using NouAssist.WorkManagement.Approval.Aggregates;
using NouAssist.WorkManagement.Approval.Entities;
using NouAssist.WorkManagement.Approval.Requests.Dtos;
using NouAssist.WorkManagement.Approval.Workflows.Dtos;

namespace NouAssist.WorkManagement.Approval
{
    public class ApprovalApplicationAutoMapperProfile : Profile
    {
        public ApprovalApplicationAutoMapperProfile()
        {
            // Workflow mappings
            CreateMap<ApprovalWorkflow, ApprovalWorkflowDto>();
            CreateMap<ApprovalStep, ApprovalStepDto>()
                .ForMember(dest => dest.ApproverUserIds, 
                    opt => opt.MapFrom(src => src.ApproverUserIds))
                .ForMember(dest => dest.ApproverRoleIds, 
                    opt => opt.MapFrom(src => src.ApproverRoleIds))
                .ForMember(dest => dest.ApproverTeamIds, 
                    opt => opt.MapFrom(src => src.ApproverTeamIds));

            // Request mappings
            CreateMap<ApprovalRequest, ApprovalRequestDto>()
                .ForMember(dest => dest.IsOverdue, opt => opt.MapFrom(src => src.IsOverdue))
                .ForMember(dest => dest.WorkflowName, opt => opt.Ignore())
                .ForMember(dest => dest.RequestedByName, opt => opt.Ignore())
                .ForMember(dest => dest.CurrentStepName, opt => opt.Ignore());

            CreateMap<ApprovalDecision, ApprovalDecisionDto>()
                .ForMember(dest => dest.StepName, opt => opt.Ignore())
                .ForMember(dest => dest.ApproverName, opt => opt.Ignore());
        }
    }
}
