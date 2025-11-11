using AutoMapper;
using NouAssist.Approval.Aggregates;
using NouAssist.Approval.Entities;
using NouAssist.Approval.Requests.Dtos;
using NouAssist.Approval.Workflows.Dtos;

namespace NouAssist.Approval
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
