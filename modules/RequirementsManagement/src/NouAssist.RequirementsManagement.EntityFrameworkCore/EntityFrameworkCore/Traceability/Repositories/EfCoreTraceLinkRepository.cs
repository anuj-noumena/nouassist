using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.RequirementsManagement.Traceability.Aggregates;
using NouAssist.RequirementsManagement.Traceability.Enums;
using NouAssist.RequirementsManagement.Traceability.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.RequirementsManagement.Traceability.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// EF Core implementation of ITraceLinkRepository
    /// </summary>
    public class EfCoreTraceLinkRepository 
        : EfCoreRepository<ITraceabilityManagementDbContext, TraceLink, Guid>, ITraceLinkRepository
    {
        public EfCoreTraceLinkRepository(
            IDbContextProvider<ITraceabilityManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public override async Task<IQueryable<TraceLink>> WithDetailsAsync()
        {
            // Always include source and target artifact references
            return (await GetQueryableAsync())
                .Include(x => x.SourceArtifact)
                .Include(x => x.TargetArtifact);
        }

        /// <inheritdoc />
        public async Task<TraceLink> FindByArtifactsAsync(
            string sourceArtifactId, 
            string targetArtifactId, 
            LinkType linkType,
            CancellationToken cancellationToken = default)
        {
            var query = await WithDetailsAsync();
            
            return await query
                .Where(link => 
                    link.SourceArtifact.ArtifactIdentifier.Id == sourceArtifactId &&
                    link.TargetArtifact.ArtifactIdentifier.Id == targetArtifactId &&
                    link.LinkType == linkType)
                .FirstOrDefaultAsync(cancellationToken);
        }

        /// <inheritdoc />
        public async Task<List<TraceLink>> GetLinksForArtifactAsync(
            string artifactId, 
            ArtifactType artifactType = ArtifactType.Other,
            CancellationToken cancellationToken = default)
        {
            var query = await WithDetailsAsync();
            
            return await query
                .Where(link => 
                    (link.SourceArtifact.ArtifactIdentifier.Id == artifactId && 
                     (artifactType == ArtifactType.Other || link.SourceArtifact.ArtifactIdentifier.Type == artifactType)) ||
                    (link.TargetArtifact.ArtifactIdentifier.Id == artifactId && 
                     (artifactType == ArtifactType.Other || link.TargetArtifact.ArtifactIdentifier.Type == artifactType)))
                .ToListAsync(cancellationToken);
        }

        /// <inheritdoc />
        public async Task<List<TraceLink>> GetSuspectLinksAsync(
            int maxResultCount = 50,
            CancellationToken cancellationToken = default)
        {
            var query = await WithDetailsAsync();
            
            return await query
                .Where(link => link.IsSuspect)
                .OrderByDescending(link => link.LastModificationTime ?? link.CreationTime)
                .Take(maxResultCount)
                .ToListAsync(cancellationToken);
        }

        /// <inheritdoc />
        public async Task<bool> ExistsAsync(
            string sourceArtifactId, 
            string targetArtifactId, 
            LinkType linkType,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .AnyAsync(link => 
                    link.SourceArtifact.ArtifactIdentifier.Id == sourceArtifactId &&
                    link.TargetArtifact.ArtifactIdentifier.Id == targetArtifactId &&
                    link.LinkType == linkType,
                    cancellationToken);
        }
    }
}
