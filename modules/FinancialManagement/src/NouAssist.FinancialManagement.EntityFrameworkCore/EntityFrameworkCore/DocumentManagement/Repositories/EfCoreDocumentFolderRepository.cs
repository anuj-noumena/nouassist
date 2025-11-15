using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.DocumentManagement.Aggregates;
using NouAssist.DocumentManagement.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.DocumentManagement.EntityFrameworkCore.Repositories
{
    public class EfCoreDocumentFolderRepository
        : EfCoreRepository<IDocumentManagementDbContext, DocumentFolder, Guid>,
          IDocumentFolderRepository
    {
        public EfCoreDocumentFolderRepository(
            IDbContextProvider<IDocumentManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<DocumentFolder>> GetChildFoldersAsync(
            Guid parentFolderId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .Where(f => f.ParentFolderId == parentFolderId)
                .OrderBy(f => f.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<DocumentFolder>> GetRootFoldersAsync(
            Guid? tenantId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .Where(f => f.ParentFolderId == null && f.TenantId == tenantId)
                .OrderBy(f => f.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<DocumentFolder> FindByPathAsync(
            string path,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query.FirstOrDefaultAsync(
                f => f.Path == path,
                cancellationToken);
        }

        public async Task<List<DocumentFolder>> SearchAsync(
            string searchTerm,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(f =>
                    f.Name.ToLower().Contains(searchTerm) ||
                    f.Path.ToLower().Contains(searchTerm) ||
                    f.Description.ToLower().Contains(searchTerm));
            }

            return await query
                .OrderBy(f => f.Level)
                .ThenBy(f => f.Name)
                .Take(100)
                .ToListAsync(cancellationToken);
        }
    }
}
