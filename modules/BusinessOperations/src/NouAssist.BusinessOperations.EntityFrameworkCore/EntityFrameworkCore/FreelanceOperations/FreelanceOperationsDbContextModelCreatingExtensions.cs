using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.BusinessOperations.FreelanceOperations.EntityFrameworkCore
{
    public static class FreelanceOperationsDbContextModelCreatingExtensions
    {
        public static void ConfigureFreelanceOperations(
            this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            // Apply all IEntityTypeConfiguration classes defined in this assembly
            builder.ApplyConfigurationsFromAssembly(typeof(FreelanceOperationsDbContext).Assembly);
        }
    }
}
