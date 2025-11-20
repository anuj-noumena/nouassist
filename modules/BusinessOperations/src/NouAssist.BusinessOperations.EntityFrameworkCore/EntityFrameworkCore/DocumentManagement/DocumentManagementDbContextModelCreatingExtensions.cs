using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.BusinessOperations.DocumentManagement.EntityFrameworkCore
{
    public static class DocumentManagementDbContextModelCreatingExtensions
    {
        public static void ConfigureDocumentManagement(
            this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            // Apply all IEntityTypeConfiguration classes in the assembly
            builder.ApplyConfigurationsFromAssembly(typeof(DocumentManagementDbContext).Assembly);

            // Any other module-specific model configurations can go here
        }
    }
}
