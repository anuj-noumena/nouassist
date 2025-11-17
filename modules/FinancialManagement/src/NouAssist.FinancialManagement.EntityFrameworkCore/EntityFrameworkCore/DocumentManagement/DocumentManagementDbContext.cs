using Microsoft.EntityFrameworkCore;
using NouAssist.FinancialManagement.DocumentManagement.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.DocumentManagement.EntityFrameworkCore
{
    [ConnectionStringName(DocumentManagementDbProperties.ConnectionStringName)]
    public class DocumentManagementDbContext
        : AbpDbContext<DocumentManagementDbContext>, IDocumentManagementDbContext
    {
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentFolder> DocumentFolders { get; set; }

        public DocumentManagementDbContext(DbContextOptions<DocumentManagementDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureDocumentManagement(this);
        }
    }
}
