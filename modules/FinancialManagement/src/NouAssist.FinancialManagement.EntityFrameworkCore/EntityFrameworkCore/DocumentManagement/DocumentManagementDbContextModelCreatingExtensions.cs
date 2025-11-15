using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.DocumentManagement.EntityFrameworkCore
{
    public static class DocumentManagementDbContextModelCreatingExtensions
    {
        public static void ConfigureDocumentManagement(
            this ModelBuilder builder,
            DocumentManagementDbContext dbContext)
        {
            Check.NotNull(builder, nameof(builder));

            builder.ApplyConfigurationsFromAssembly(typeof(DocumentManagementDbContext).Assembly);
        }
    }
}
