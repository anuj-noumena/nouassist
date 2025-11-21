using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.RequirementsManagement.Specification.Enums;
using NouAssist.RequirementsManagement.Specification.Specifications;
using NouAssist.RequirementsManagement.Specification.ValueObjects;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.RequirementsManagement.Specification.Repositories
{
    /// <summary>
    /// Repository interface for Requirement aggregate
    /// </summary>
    public interface IRequirementRepository : IRepository<Requirement, Guid>
    {
        Task<Requirement> FindByIdentifierAsync(RequirementIdentifier identifier, CancellationToken cancellationToken = default);
        
        Task<List<Requirement>> GetListAsync(
            RequirementSpecification specification,
            CancellationToken cancellationToken = default);

        Task<int> CountAsync(
            RequirementSpecification specification,
            CancellationToken cancellationToken = default);

        Task<bool> AnyAsync(
            RequirementSpecification specification,
            CancellationToken cancellationToken = default);

        Task<List<Requirement>> GetByBusinessObjectiveAsync(
            Guid objectiveId,
            CancellationToken cancellationToken = default);

        Task<List<Requirement>> GetByScopeStatusAsync(
            ScopeStatus scopeStatus,
            CancellationToken cancellationToken = default);
    }
}
