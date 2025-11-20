using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.RequirementsManagement.Specification.Aggregates;
using NouAssist.RequirementsManagement.Specification.Enums;
using NouAssist.RequirementsManagement.Specification.Repositories;
using NouAssist.RequirementsManagement.Specification.Specifications;
using NouAssist.RequirementsManagement.Specification.ValueObjects;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.RequirementsManagement.Specification.EntityFrameworkCore.Repositories
{
    public class EfCoreRequirementRepository 
        : EfCoreRepository<IRequirementSpecificationManagementDbContext, Requirement, Guid>, IRequirementRepository
    {
        public EfCoreRequirementRepository(
            IDbContextProvider<IRequirementSpecificationManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Requirement> FindByIdentifierAsync(RequirementIdentifier identifier, CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(r => r.Identifier.Value == identifier.Value)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<Requirement>> GetListAsync(
            RequirementSpecification specification,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(specification.ToExpression())
                .ToListAsync(cancellationToken);
        }

        public async Task<int> CountAsync(
            RequirementSpecification specification,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(specification.ToExpression())
                .CountAsync(cancellationToken);
        }

        public async Task<bool> AnyAsync(
            RequirementSpecification specification,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(specification.ToExpression())
                .AnyAsync(cancellationToken);
        }

        public async Task<List<Requirement>> GetByBusinessObjectiveAsync(
            Guid objectiveId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(r => r.BusinessObjectiveLinks.Any(bol => bol.ObjectiveId.Value == objectiveId))
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Requirement>> GetByScopeStatusAsync(
            ScopeStatus scopeStatus,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(r => r.ScopeMarker != null && r.ScopeMarker.ScopeStatus == scopeStatus)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Override to always include child entities by default
        /// </summary>
        public override async Task<IQueryable<Requirement>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(r => r.AcceptanceCriteria)
                .Include(r => r.AssociatedRules)
                .Include(r => r.AssociatedConstraints)
                .Include(r => r.DataModelReferences)
                .Include(r => r.InterfaceSpecifications)
                .Include(r => r.BusinessObjectiveLinks)
                .Include(r => r.ScopeMarker);
        }

        /// <summary>
        /// Get requirements with pagination and sorting
        /// </summary>
        public async Task<List<Requirement>> GetPagedListAsync(
            RequirementSpecification specification,
            int skipCount,
            int maxResultCount,
            string sorting = null,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = includeDetails 
                ? await WithDetailsAsync() 
                : await GetQueryableAsync();

            query = query.Where(specification.ToExpression());

            if (!sorting.IsNullOrWhiteSpace())
            {
                query = query.OrderBy(sorting);
            }
            else
            {
                query = query.OrderBy(r => r.Identifier.Value);
            }

            return await query
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Search requirements by text
        /// </summary>
        public async Task<List<Requirement>> SearchAsync(
            string searchText,
            RequirementType? type = null,
            RequirementStatus? status = null,
            RequirementPriority? priority = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            if (!searchText.IsNullOrWhiteSpace())
            {
                query = query.Where(r => 
                    r.Identifier.Value.Contains(searchText) ||
                    r.Text.Title.Contains(searchText) ||
                    r.Text.Description.Contains(searchText));
            }

            if (type.HasValue)
            {
                query = query.Where(r => r.Type == type.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(r => r.Status == status.Value);
            }

            if (priority.HasValue)
            {
                query = query.Where(r => r.Priority == priority.Value);
            }

            return await query
                .OrderBy(r => r.Identifier.Value)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Get requirements created by a specific user
        /// </summary>
        public async Task<List<Requirement>> GetByCreatorAsync(
            Guid creatorId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(r => r.CreatorId == creatorId)
                .OrderByDescending(r => r.CreationTime)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Get requirements that were modified after a specific date
        /// </summary>
        public async Task<List<Requirement>> GetModifiedSinceAsync(
            DateTime sinceDateTime,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(r => r.LastModificationTime > sinceDateTime)
                .OrderByDescending(r => r.LastModificationTime)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Get requirements statistics
        /// </summary>
        public async Task<RequirementStatistics> GetStatisticsAsync(CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            var statistics = new RequirementStatistics
            {
                Total = await query.CountAsync(cancellationToken),
                ByStatus = await query
                    .GroupBy(r => r.Status)
                    .Select(g => new { Status = g.Key, Count = g.Count() })
                    .ToDictionaryAsync(x => x.Status, x => x.Count, cancellationToken),
                ByType = await query
                    .GroupBy(r => r.Type)
                    .Select(g => new { Type = g.Key, Count = g.Count() })
                    .ToDictionaryAsync(x => x.Type, x => x.Count, cancellationToken),
                ByPriority = await query
                    .GroupBy(r => r.Priority)
                    .Select(g => new { Priority = g.Key, Count = g.Count() })
                    .ToDictionaryAsync(x => x.Priority, x => x.Count, cancellationToken)
            };

            return statistics;
        }
    }

    /// <summary>
    /// DTO for requirement statistics
    /// </summary>
    public class RequirementStatistics
    {
        public int Total { get; set; }
        public Dictionary<RequirementStatus, int> ByStatus { get; set; } = new();
        public Dictionary<RequirementType, int> ByType { get; set; } = new();
        public Dictionary<RequirementPriority, int> ByPriority { get; set; } = new();
    }
}
