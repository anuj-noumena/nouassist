using Microsoft.EntityFrameworkCore;
using NouAssist.RequirementsManagement.ValidationVerification.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.RequirementsManagement.ValidationVerification.EntityFrameworkCore
{
    [ConnectionStringName(ValidationVerificationDbProperties.ConnectionStringName)]
    public class ValidationVerificationDbContext 
        : AbpDbContext<ValidationVerificationDbContext>, IValidationVerificationDbContext 
    {
        // Implement DbSet properties from the interface
        public DbSet<RequirementReviewCycle> RequirementReviewCycles { get; set; }
        public DbSet<TestCase> TestCases { get; set; }

        public ValidationVerificationDbContext(DbContextOptions<ValidationVerificationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // IMPORTANT: Call base first to apply ABP conventions
            base.OnModelCreating(builder); 

            // Apply all configurations defined in this assembly using the extension method
            builder.ConfigureValidationVerification(this); 
        }
    }
}
