using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Aggregates;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Repositories;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Specifications;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.EntityFrameworkCore.Repositories
{
    public class EfCoreGlossaryTermRepository 
        : EfCoreRepository<IRequirementsAnalysisToolsDbContext, GlossaryTerm, Guid>, 
          IGlossaryTermRepository
    {
        public EfCoreGlossaryTermRepository(
            IDbContextProvider<IRequirementsAnalysisToolsDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<GlossaryTerm?> FindByNameAsync(
            string name,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .FirstOrDefaultAsync(gt => gt.Name == name, cancellationToken);
        }

        public async Task<bool> ExistsByNameAsync(
            string name,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .AnyAsync(gt => gt.Name == name, cancellationToken);
        }

        public async Task<List<GlossaryTerm>> GetListAsync(
            Specification<GlossaryTerm> specification,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(specification.ToExpression())
                .OrderBy(gt => gt.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<GlossaryTerm>> GetApprovedTermsAsync(
            CancellationToken cancellationToken = default)
        {
            var specification = new ApprovedTermsSpecification();
            return await GetListAsync(specification, cancellationToken);
        }

        public async Task<List<GlossaryTerm>> GetPendingApprovalAsync(
            CancellationToken cancellationToken = default)
        {
            var specification = new PendingApprovalSpecification();
            return await GetListAsync(specification, cancellationToken);
        }

        public async Task<List<GlossaryTerm>> SearchByNameAsync(
            string searchTerm,
            int maxResults = 50,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(gt => EF.Functions.ILike(gt.Name, $"%{searchTerm}%") ||
                            EF.Functions.ILike(gt.Definition, $"%{searchTerm}%") ||
                            (gt.Synonyms != null && EF.Functions.ILike(gt.Synonyms, $"%{searchTerm}%")) ||
                            (gt.Acronyms != null && EF.Functions.ILike(gt.Acronyms, $"%{searchTerm}%")))
                .OrderBy(gt => gt.Name)
                .Take(maxResults)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<GlossaryTerm>> GetRelatedTermsAsync(
            List<Guid> termIds,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(gt => termIds.Contains(gt.Id))
                .OrderBy(gt => gt.Name)
                .ToListAsync(cancellationToken);
        }

        public override async Task<List<GlossaryTerm>> GetListAsync(
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .OrderBy(gt => gt.Name)
                .ToListAsync(cancellationToken);
        }

        public override async Task<List<GlossaryTerm>> GetPagedListAsync(
            int skipCount,
            int maxResultCount,
            string sorting,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(GlossaryTerm.Name) : sorting)
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(cancellationToken);
        }
    }
}
