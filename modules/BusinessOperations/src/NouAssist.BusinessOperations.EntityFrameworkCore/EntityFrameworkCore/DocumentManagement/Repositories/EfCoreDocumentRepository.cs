using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.BusinessOperations.DocumentManagement.Aggregates;
using NouAssist.BusinessOperations.DocumentManagement.Entities;
using NouAssist.BusinessOperations.DocumentManagement.Enums;
using NouAssist.BusinessOperations.DocumentManagement.Repositories;
using NouAssist.BusinessOperations.DocumentManagement.Specifications;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.BusinessOperations.DocumentManagement.EntityFrameworkCore.Repositories
{
    public class EfCoreDocumentRepository 
        : EfCoreRepository<DocumentManagementDbContext, Document, Guid>, IDocumentRepository
    {
        public EfCoreDocumentRepository(
            IDbContextProvider<DocumentManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public virtual async Task<List<Document>> GetListAsync(
            DocumentSpecification specification,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            // Apply specification
            query = query.Where(specification.ToExpression());
            
            return await query.ToListAsync(cancellationToken);
        }
        
        public virtual async Task<int> CountAsync(
            DocumentSpecification specification,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            // Apply specification
            query = query.Where(specification.ToExpression());
            
            return await query.CountAsync(cancellationToken);
        }
        
        public virtual async Task<bool> AnyAsync(
            DocumentSpecification specification,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            // Apply specification
            query = query.Where(specification.ToExpression());
            
            return await query.AnyAsync(cancellationToken);
        }
        
        public override async Task<IQueryable<Document>> WithDetailsAsync()
        {
            // Include the necessary navigation properties
            return (await GetQueryableAsync())
                .Include(d => d.Versions);
        }
        
        public async Task<List<Document>> GetDocumentsForUserAsync(
            Guid userId,
            IEnumerable<Guid> userRoleIds,
            AccessPermissionLevel minPermissionLevel,
            DocumentStatus? status = null,
            int skipCount = 0,
            int maxResultCount = 10,
            string sorting = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            // Filter by access grants
            query = query.Where(d => 
                // Direct user access
                d.AccessGrants.Any(a => 
                    a.PrincipalId == userId && 
                    !a.IsRole && 
                    a.PermissionLevel >= minPermissionLevel) ||
                // Role-based access
                d.AccessGrants.Any(a => 
                    userRoleIds.Contains(a.PrincipalId) && 
                    a.IsRole && 
                    a.PermissionLevel >= minPermissionLevel)
            );
            
            // Filter by status if specified
            if (status.HasValue)
            {
                query = query.Where(d => d.Status == status.Value);
            }
            
            // Apply sorting
            sorting ??= nameof(Document.CreationTime) + " DESC";
            query = query.OrderBy(sorting);
            
            // Apply paging
            query = query.Skip(skipCount).Take(maxResultCount);
            
            return await query.ToListAsync(cancellationToken);
        }
        
        public async Task<Document> GetDocumentWithVersionsAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Include(d => d.Versions)
                .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        }
        
        public async Task<List<DocumentVersion>> GetDocumentVersionsAsync(
            Guid documentId,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.DocumentVersions
                .Where(v => v.DocumentId == documentId)
                .OrderByDescending(v => v.VersionNumber.Major)
                .ThenByDescending(v => v.VersionNumber.Minor)
                .ToListAsync(cancellationToken);
        }
    }
}
