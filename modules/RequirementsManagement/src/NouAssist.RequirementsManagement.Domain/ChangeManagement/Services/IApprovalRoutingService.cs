using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.RequirementsManagement.ChangeManagement.Aggregates;
using NouAssist.RequirementsManagement.ChangeManagement.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.RequirementsManagement.ChangeManagement.Services
{
    /// <summary>
    /// Service for managing approval routing and workflow
    /// </summary>
    public interface IApprovalRoutingService : IDomainService
    {
        /// <summary>
        /// Determines the required approval steps for a change request
        /// </summary>
        Task<List<ApprovalStepDefinition>> DetermineApprovalStepsAsync(
            RequirementChangeRequest changeRequest,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Routes a change request to appropriate approvers
        /// </summary>
        Task RouteForApprovalAsync(
            RequirementChangeRequest changeRequest,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the next approver in the approval chain
        /// </summary>
        Task<Guid?> GetNextApproverAsync(
            RequirementChangeRequest changeRequest,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Validates if a user can approve a specific change request
        /// </summary>
        Task<bool> CanUserApproveAsync(
            Guid userId,
            RequirementChangeRequest changeRequest,
            CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Defines an approval step requirement
    /// </summary>
    public class ApprovalStepDefinition
    {
        public ApproverRole ApproverRole { get; set; }
        public Guid ApproverId { get; set; }
        public int SequenceOrder { get; set; }
        public bool IsRequired { get; set; } = true;
    }
}
