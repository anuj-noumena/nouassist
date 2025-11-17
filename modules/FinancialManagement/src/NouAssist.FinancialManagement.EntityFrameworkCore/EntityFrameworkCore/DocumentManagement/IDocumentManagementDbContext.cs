using Microsoft.EntityFrameworkCore;
using NouAssist.FinancialManagement.DocumentManagement.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.DocumentManagement.EntityFrameworkCore
{
    [ConnectionStringName(DocumentManagementDbProperties.ConnectionStringName)]
    public interface IDocumentManagementDbContext : IEfCoreDbContext
    {
        DbSet<Document> Documents { get; }
        DbSet<DocumentFolder> DocumentFolders { get; }
    }
}
