using Microsoft.EntityFrameworkCore;
using NouAssist.BusinessOperations.DocumentManagement.Aggregates;
using NouAssist.BusinessOperations.DocumentManagement.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.BusinessOperations.DocumentManagement.EntityFrameworkCore
{
    [ConnectionStringName(DocumentManagementDbProperties.ConnectionStringName)]
    public class DocumentManagementDbContext 
        : AbpDbContext<DocumentManagementDbContext>, IDocumentManagementDbContext
    {
        // Implement DbSet properties from the interface
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentVersion> DocumentVersions { get; set; }

        public DocumentManagementDbContext(DbContextOptions<DocumentManagementDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // IMPORTANT: Call base first to apply ABP conventions
            base.OnModelCreating(builder);

            // Apply all configurations defined in this assembly
            builder.ConfigureDocumentManagement();
        }
    }
}
