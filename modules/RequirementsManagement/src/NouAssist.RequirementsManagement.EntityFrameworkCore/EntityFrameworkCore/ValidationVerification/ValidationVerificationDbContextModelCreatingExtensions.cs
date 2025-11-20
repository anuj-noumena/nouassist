using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.RequirementsManagement.ValidationVerification.EntityFrameworkCore
{
    public static class ValidationVerificationDbContextModelCreatingExtensions
    {
        public static void ConfigureValidationVerification(
            this ModelBuilder builder, 
            ValidationVerificationDbContext dbContext)
        {
            Check.NotNull(builder, nameof(builder));

            // Apply all IEntityTypeConfiguration classes defined in this assembly
            builder.ApplyConfigurationsFromAssembly(typeof(ValidationVerificationDbContext).Assembly);

            // Module-specific configuration could go here if needed
        }
    }
}
