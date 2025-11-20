using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.EntityFrameworkCore
{
    public static class KnowledgeInnovationDbContextModelCreatingExtensions
    {
        public static void ConfigureKnowledgeInnovation(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            // Apply all IEntityTypeConfiguration classes in the assembly
            builder.ApplyConfigurationsFromAssembly(typeof(KnowledgeInnovationDbContext).Assembly);
        }
    }
}
