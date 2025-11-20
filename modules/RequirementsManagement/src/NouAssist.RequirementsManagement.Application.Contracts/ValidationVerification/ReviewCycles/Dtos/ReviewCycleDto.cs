using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using NouAssist.RequirementsManagement.ValidationVerification.Enums;

namespace NouAssist.RequirementsManagement.ValidationVerification.ReviewCycles.Dtos
{
    /// <summary>
    /// Data transfer object for requirement review cycle
    /// </summary>
    public class ReviewCycleDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid RequirementId { get; set; }
        public ReviewCycleStatus Status { get; set; }
        public WorkflowDefinitionDto WorkflowDefinition { get; set; }
        public DateTime? TargetCompletionDate { get; set; }
        public DateTime? ActualCompletionDate { get; set; }
        public string CompletionComments { get; set; }
        public List<ReviewerAssignmentDto> ReviewerAssignments { get; set; } = new();
        public List<ReviewFeedbackDto> FeedbackItems { get; set; } = new();
        public ReviewWorkflowInstanceDto WorkflowInstance { get; set; }
        public ReviewCycleStatisticsDto Statistics { get; set; }
    }

    /// <summary>
    /// Lightweight DTO for review cycle lists
    /// </summary>
    public class ReviewCycleSummaryDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public Guid RequirementId { get; set; }
        public ReviewCycleStatus Status { get; set; }
        public DateTime? TargetCompletionDate { get; set; }
        public DateTime CreationTime { get; set; }
        public int TotalReviewers { get; set; }
        public int CompletedReviews { get; set; }
        public bool IsOverdue { get; set; }
    }

    /// <summary>
    /// DTO for workflow definition reference
    /// </summary>
    public class WorkflowDefinitionDto
    {
        public Guid DefinitionId { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
    }

    /// <summary>
    /// DTO for reviewer assignment
    /// </summary>
    public class ReviewerAssignmentDto
    {
        public Guid ReviewerId { get; set; }
        public string ReviewerName { get; set; }
        public string ReviewerEmail { get; set; }
        public DateTime AssignedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsRequired { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsOverdue { get; set; }
    }

    /// <summary>
    /// DTO for review feedback
    /// </summary>
    public class ReviewFeedbackDto
    {
        public Guid Id { get; set; }
        public Guid ReviewerId { get; set; }
        public string ReviewerName { get; set; }
        public ReviewDecisionType DecisionType { get; set; }
        public string DecisionReason { get; set; }
        public DateTime DecisionDate { get; set; }
        public string Comment { get; set; }
        public DateTime SubmittedAt { get; set; }
    }

    /// <summary>
    /// DTO for review workflow instance
    /// </summary>
    public class ReviewWorkflowInstanceDto
    {
        public Guid Id { get; set; }
        public WorkflowDefinitionDto WorkflowDefinition { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public ReviewCycleStatus? FinalStatus { get; set; }
        public string CompletionReason { get; set; }
        public string WorkflowState { get; set; }
        public TimeSpan? Duration { get; set; }
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// DTO for review cycle statistics
    /// </summary>
    public class ReviewCycleStatisticsDto
    {
        public int TotalReviewers { get; set; }
        public int RequiredReviewers { get; set; }
        public int CompletedReviews { get; set; }
        public int PendingReviews { get; set; }
        public int ApprovedCount { get; set; }
        public int RejectedCount { get; set; }
        public int ChangesRequestedCount { get; set; }
        public int OverdueReviews { get; set; }
        public double CompletionPercentage { get; set; }
    }
}
