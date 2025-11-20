using NouAssist.RequirementsManagement.ChangeManagement.Aggregates;
using NouAssist.RequirementsManagement.ChangeManagement.ValueObjects;
using NouAssist.RequirementsManagement.ChangeManagement.Specifications;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.RequirementsManagement.ChangeManagement.Repositories
{
    /// <summary>
    /// Repository for RequirementChangeRequest aggregate
    /// </summary>
    public interface IRequirementChangeRequestRepository : IRepository<RequirementChangeRequest, Guid>
    {
        /// <summary>
        /// Finds change requests matching the given specification
        /// </summary>
        Task<List<RequirementChangeRequest>> GetListAsync(
            ChangeRequestSpecification specification,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Counts change requests matching the given specification
        /// </summary>
        Task<int> CountAsync(
            ChangeRequestSpecification specification,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if any change requests match the given specification
        /// </summary>
        Task<bool> AnyAsync(
            ChangeRequestSpecification specification,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds a change request by its request identifier
        /// </summary>
        Task<RequirementChangeRequest?> FindByRequestIdentifierAsync(
            RequestIdentifier requestIdentifier,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets change requests affecting a specific requirement
        /// </summary>
        Task<List<RequirementChangeRequest>> GetByAffectedRequirementAsync(
            Guid requirementId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets change requests submitted by a specific user
        /// </summary>
        Task<List<RequirementChangeRequest>> GetBySubmitterAsync(
            Guid submitterId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets change requests requiring approval from a specific user
        /// </summary>
        Task<List<RequirementChangeRequest>> GetPendingForApprovalByUserAsync(
            Guid approverId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets change requests with high impact that require attention
        /// </summary>
        Task<List<RequirementChangeRequest>> GetHighImpactChangesAsync(
            decimal costThreshold = 10000m,
            int timeThreshold = 30,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets overdue change requests
        /// </summary>
        Task<List<RequirementChangeRequest>> GetOverdueChangesAsync(
            int overdueDays = 30,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Ensures the request identifier is unique
        /// </summary>
        Task<bool> IsRequestIdentifierUniqueAsync(
            RequestIdentifier requestIdentifier,
            Guid? excludeId = null,
            CancellationToken cancellationToken = default);
    }
}
