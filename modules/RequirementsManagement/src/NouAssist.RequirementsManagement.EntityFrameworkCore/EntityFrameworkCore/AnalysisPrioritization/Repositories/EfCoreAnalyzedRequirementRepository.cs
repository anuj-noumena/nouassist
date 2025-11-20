using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Aggregates;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Repositories;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Enums;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Specifications;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.EntityFrameworkCore.Repositories
{
    public class EfCoreAnalyzedRequirementRepository 
        : EfCoreRepository<IRequirementsAnalysisToolsDbContext, AnalyzedRequirement, Guid>, 
          IAnalyzedRequirementRepository
    {
        public EfCoreAnalyzedRequirementRepository(
            IDbContextProvider<IRequirementsAnalysisToolsDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<AnalyzedRequirement?> FindByRequirementIdAsync(
            Guid requirementId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Include(ar => ar.Attributes)
                .Include(ar => ar.Relationships)
                .FirstOrDefaultAsync(ar => ar.RequirementId == requirementId, cancellationToken);
        }

        public async Task<List<AnalyzedRequirement>> GetListAsync(
            Specification<AnalyzedRequirement> specification,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(specification.ToExpression())
                .Include(ar => ar.Attributes)
                .Include(ar => ar.Relationships)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<AnalyzedRequirement>> GetRequirementsWithAttributeAsync(
            AttributeType attributeType,
            CancellationToken cancellationToken = default)
        {
            var specification = new HasAttributeTypeSpecification(attributeType);
            return await GetListAsync(specification, cancellationToken);
        }

        public async Task<List<AnalyzedRequirement>> GetRequirementsWithRelationshipTypeAsync(
            RelationshipType relationshipType,
            CancellationToken cancellationToken = default)
        {
            var specification = new HasRelationshipTypeSpecification(relationshipType);
            return await GetListAsync(specification, cancellationToken);
        }

        public async Task<List<AnalyzedRequirement>> GetRequirementsWithoutImpactAssessmentAsync(
            CancellationToken cancellationToken = default)
        {
            var specification = new MissingImpactAssessmentSpecification();
            return await GetListAsync(specification, cancellationToken);
        }

        public async Task<int> CountAsync(
            Specification<AnalyzedRequirement> specification,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(specification.ToExpression())
                .CountAsync(cancellationToken);
        }

        public override async Task<IQueryable<AnalyzedRequirement>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(ar => ar.Attributes)
                .Include(ar => ar.Relationships);
        }

        public override async Task<AnalyzedRequirement> GetAsync(
            Guid id,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            if (includeDetails)
            {
                query = query
                    .Include(ar => ar.Attributes)
                    .Include(ar => ar.Relationships);
            }

            var analyzedRequirement = await query
                .FirstOrDefaultAsync(ar => ar.Id == id, cancellationToken);

            if (analyzedRequirement == null)
            {
                throw new EntityNotFoundException(typeof(AnalyzedRequirement), id);
            }

            return analyzedRequirement;
        }

        public override async Task<AnalyzedRequirement?> FindAsync(
            Guid id,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            if (includeDetails)
            {
                query = query
                    .Include(ar => ar.Attributes)
                    .Include(ar => ar.Relationships);
            }

            return await query
                .FirstOrDefaultAsync(ar => ar.Id == id, cancellationToken);
        }
    }
}
