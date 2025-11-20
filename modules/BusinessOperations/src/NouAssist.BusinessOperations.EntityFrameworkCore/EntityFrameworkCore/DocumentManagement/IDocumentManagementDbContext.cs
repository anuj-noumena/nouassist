using Microsoft.EntityFrameworkCore;
using NouAssist.BusinessOperations.DocumentManagement.Aggregates;
using NouAssist.BusinessOperations.DocumentManagement.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.BusinessOperations.DocumentManagement.EntityFrameworkCore
{
    [ConnectionStringName(DocumentManagementDbProperties.ConnectionStringName)]
    public interface IDocumentManagementDbContext : IEfCoreDbContext
    {
        // DbSet for Aggregate Root
        DbSet<Document> Documents { get; }
        
        // DbSet for child entities if needed for direct queries
        DbSet<DocumentVersion> DocumentVersions { get; }
    }
}
