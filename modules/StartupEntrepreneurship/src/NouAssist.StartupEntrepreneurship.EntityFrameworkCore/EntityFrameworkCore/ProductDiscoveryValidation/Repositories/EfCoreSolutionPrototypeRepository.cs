using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Aggregates;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Enums;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// EF Core implementation of SolutionPrototype repository
    /// </summary>
    public class EfCoreSolutionPrototypeRepository 
        : EfCoreRepository<IProductDiscoveryDbContext, SolutionPrototype, Guid>, ISolutionPrototypeRepository
    {
        public EfCoreSolutionPrototypeRepository(
            IDbContextProvider<IProductDiscoveryDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<SolutionPrototype>> GetListAsync(
            Expression<Func<SolutionPrototype, bool>> predicate,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            if (includeDetails)
            {
                query = query.Include(p => p.Versions);
            }

            return await query
                .Where(predicate)
                .OrderByDescending(p => p.CreationTime)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<SolutionPrototype>> GetByTypeAsync(
            PrototypeType type,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await GetListAsync(
                p => p.Type == type,
                includeDetails,
                cancellationToken);
        }

        public async Task<List<SolutionPrototype>> GetByHypothesisAsync(
            Guid hypothesisId,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await GetListAsync(
                p => p.RelatedHypothesisId == hypothesisId,
                includeDetails,
                cancellationToken);
        }

        public async Task<SolutionPrototype> GetWithVersionsAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            var prototype = await query
                .Include(p => p.Versions)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            if (prototype == null)
                throw new EntityNotFoundException(typeof(SolutionPrototype), id);

            return prototype;
        }

        public async Task<List<SolutionPrototype>> GetTestReadyAsync(
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .Include(p => p.Versions)
                .Where(p => p.Versions.Any(v => v.IsActive) &&
                           p.Type == PrototypeType.InteractivePrototype)
                .OrderByDescending(p => p.CreationTime)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<SolutionPrototype>> GetIteratedPrototypesAsync(
            int minimumVersions = 2,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .Include(p => p.Versions)
                .Where(p => p.Versions.Count >= minimumVersions)
                .OrderByDescending(p => p.Versions.Count)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<SolutionPrototype>> GetStalePrototypesAsync(
            int daysSinceLastUpdate = 60,
            CancellationToken cancellationToken = default)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-daysSinceLastUpdate);
            var query = await GetQueryableAsync();

            return await query
                .Include(p => p.Versions)
                .Where(p => p.Versions.Any() &&
                           p.Versions.All(v => v.CreatedDate <= cutoffDate))
                .OrderByDescending(p => p.Versions.Max(v => v.CreatedDate))
                .ToListAsync(cancellationToken);
        }

        public async Task<PrototypeStatistics> GetStatisticsAsync(
            DateTime? fromDate = null,
            DateTime? toDate = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            if (fromDate.HasValue)
                query = query.Where(p => p.CreationTime >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(p => p.CreationTime <= toDate.Value);

            var prototypes = await query
                .Include(p => p.Versions)
                .ToListAsync(cancellationToken);

            var totalPrototypes = prototypes.Count;
            
            var prototypesByType = prototypes
                .GroupBy(p => p.Type)
                .ToDictionary(g => g.Key, g => g.Count());

            var prototypesWithMultipleVersions = prototypes.Count(p => p.Versions.Count > 1);
            var prototypesReadyForTesting = prototypes.Count(p => 
                p.Versions.Any(v => v.IsActive) && p.Type == PrototypeType.InteractivePrototype);
            var stalePrototypes = prototypes.Count(p => 
                p.Versions.Any() && p.Versions.All(v => v.CreatedDate <= DateTime.UtcNow.AddDays(-60)));

            var avgVersionsPerPrototype = prototypes.Any() 
                ? prototypes.Average(p => p.Versions.Count) 
                : 0;

            var prototypesByHypothesis = prototypes
                .Where(p => p.RelatedHypothesisId.HasValue)
                .GroupBy(p => p.RelatedHypothesisId.Value)
                .ToDictionary(g => g.Key.ToString(), g => g.Count());

            return new PrototypeStatistics
            {
                TotalPrototypes = totalPrototypes,
                PrototypesByType = prototypesByType,
                PrototypesWithMultipleVersions = prototypesWithMultipleVersions,
                PrototypesReadyForTesting = prototypesReadyForTesting,
                StalePrototypes = stalePrototypes,
                AverageVersionsPerPrototype = avgVersionsPerPrototype,
                PrototypesByHypothesis = prototypesByHypothesis
            };
        }

        public async Task<List<SolutionPrototype>> SearchAsync(
            string searchTerm,
            int maxResults = 20,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new List<SolutionPrototype>();

            var query = await GetQueryableAsync();
            searchTerm = searchTerm.ToLower();

            return await query
                .Where(p => p.Name.ToLower().Contains(searchTerm) ||
                           p.Description.ToLower().Contains(searchTerm) ||
                           p.Purpose.ToLower().Contains(searchTerm))
                .OrderByDescending(p => p.CreationTime)
                .Take(maxResults)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<SolutionPrototype>> GetReadyForTestingAsync(
            CancellationToken cancellationToken = default)
        {
            return await GetTestReadyAsync(cancellationToken);
        }

        public async Task<Dictionary<PrototypeType, int>> GetTypeDistributionAsync(
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .GroupBy(p => p.Type)
                .ToDictionaryAsync(g => g.Key, g => g.Count(), cancellationToken);
        }

        public override async Task<IQueryable<SolutionPrototype>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(p => p.Versions);
        }
    }
}
