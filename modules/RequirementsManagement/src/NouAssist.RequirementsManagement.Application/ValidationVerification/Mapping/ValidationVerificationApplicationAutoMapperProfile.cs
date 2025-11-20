using AutoMapper;
using NouAssist.RequirementsManagement.ValidationVerification.Aggregates;
using NouAssist.RequirementsManagement.ValidationVerification.Entities;
using NouAssist.RequirementsManagement.ValidationVerification.ValueObjects;
using NouAssist.RequirementsManagement.ValidationVerification.Services;
using NouAssist.RequirementsManagement.ValidationVerification.ReviewCycles.Dtos;
using NouAssist.RequirementsManagement.ValidationVerification.TestCases.Dtos;
using NouAssist.RequirementsManagement.ValidationVerification.TestCoverage.Dtos;

namespace NouAssist.RequirementsManagement.ValidationVerification.Mapping
{
    /// <summary>
    /// AutoMapper profile for ValidationVerification application layer
    /// </summary>
    public class ValidationVerificationApplicationAutoMapperProfile : Profile
    {
        public ValidationVerificationApplicationAutoMapperProfile()
        {
            // Review Cycle mappings
            CreateMap<RequirementReviewCycle, ReviewCycleDto>()
                .ForMember(dest => dest.Statistics, opt => opt.MapFrom(src => src.GetStatistics()));

            CreateMap<RequirementReviewCycle, ReviewCycleSummaryDto>()
                .ForMember(dest => dest.TotalReviewers, opt => opt.MapFrom(src => src.ReviewerAssignments.Count))
                .ForMember(dest => dest.CompletedReviews, opt => opt.MapFrom(src => src.FeedbackItems.Count))
                .ForMember(dest => dest.IsOverdue, opt => opt.MapFrom(src => 
                    src.Status == ReviewCycleStatus.InProgress && 
                    src.TargetCompletionDate.HasValue && 
                    src.TargetCompletionDate.Value < DateTime.UtcNow));

            CreateMap<ReviewCycleStatistics, ReviewCycleStatisticsDto>()
                .ForMember(dest => dest.CompletionPercentage, opt => opt.MapFrom(src => 
                    src.RequiredReviewers > 0 ? (double)src.CompletedReviews / src.RequiredReviewers * 100 : 0));

            // Value Object mappings
            CreateMap<WorkflowDefinitionRef, WorkflowDefinitionDto>();
            CreateMap<CreateWorkflowDefinitionDto, WorkflowDefinitionRef>()
                .ConstructUsing(dto => new WorkflowDefinitionRef(dto.DefinitionId, dto.Name, dto.Version))
                .ForAllMembers(opts => opts.Ignore());

            CreateMap<ReviewerAssignment, ReviewerAssignmentDto>()
                .ForMember(dest => dest.IsCompleted, opt => opt.Ignore()) // Set in service
                .ForMember(dest => dest.IsOverdue, opt => opt.MapFrom(src => src.IsOverdue(DateTime.UtcNow)));

            CreateMap<CreateReviewerAssignmentDto, ReviewerAssignment>()
                .ConstructUsing(dto => new ReviewerAssignment(
                    dto.ReviewerId, dto.ReviewerName, dto.ReviewerEmail, 
                    DateTime.UtcNow, dto.DueDate, dto.IsRequired))
                .ForAllMembers(opts => opts.Ignore());

            CreateMap<ReviewFeedbackItem, ReviewFeedbackDto>()
                .ForMember(dest => dest.DecisionType, opt => opt.MapFrom(src => src.Decision.DecisionType))
                .ForMember(dest => dest.DecisionReason, opt => opt.MapFrom(src => src.Decision.Reason))
                .ForMember(dest => dest.DecisionDate, opt => opt.MapFrom(src => src.Decision.DecisionDate))
                .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => 
                    src.Comment != null ? src.Comment.Comment : null));

            CreateMap<ReviewWorkflowInstance, ReviewWorkflowInstanceDto>()
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.GetDuration()))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive()));

            // Test Case mappings
            CreateMap<TestCase, TestCaseDto>()
                .ForMember(dest => dest.Statistics, opt => opt.MapFrom(src => src.GetExecutionStatistics()));

            CreateMap<TestCase, TestCaseSummaryDto>()
                .ForMember(dest => dest.StepCount, opt => opt.MapFrom(src => src.TestSteps.Count))
                .ForMember(dest => dest.RequirementCount, opt => opt.MapFrom(src => src.RequirementIds.Count))
                .ForMember(dest => dest.LastExecutionStatus, opt => opt.MapFrom(src => 
                    src.ExecutionLogs.Where(el => el.IsCompleted())
                        .OrderByDescending(el => el.CompletedAt)
                        .FirstOrDefault().Status))
                .ForMember(dest => dest.LastExecutionDate, opt => opt.MapFrom(src => 
                    src.ExecutionLogs.Where(el => el.IsCompleted())
                        .OrderByDescending(el => el.CompletedAt)
                        .FirstOrDefault().CompletedAt));

            CreateMap<TestCaseStatistics, TestCaseStatisticsDto>()
                .ForMember(dest => dest.SuccessRate, opt => opt.MapFrom(src => 
                    src.TotalExecutions > 0 ? (double)src.PassedExecutions / src.TotalExecutions * 100 : 0));

            CreateMap<TestStep, TestStepDto>();
            CreateMap<TestStepInstruction, TestStepInstructionDto>();
            CreateMap<ExpectedResult, ExpectedResultDto>();
            CreateMap<ActualResult, ActualResultDto>();

            CreateMap<TestExecutionLog, TestExecutionLogDto>()
                .ForMember(dest => dest.ExecutionDuration, opt => opt.MapFrom(src => src.ExecutionDuration));

            // Test Coverage mappings
            CreateMap<RequirementTestCoverage, RequirementTestCoverageDto>();
            CreateMap<TestExecutionStatistics, TestExecutionStatisticsDto>();
            CreateMap<TestCoverageReport, TestCoverageReportDto>();
            CreateMap<TestCoverageSummary, TestCoverageSummaryDto>();
            CreateMap<RequirementTestGap, RequirementTestGapDto>();
        }
    }
}
