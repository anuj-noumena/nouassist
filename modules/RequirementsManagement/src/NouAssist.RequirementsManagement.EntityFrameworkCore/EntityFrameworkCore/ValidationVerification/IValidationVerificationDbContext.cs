using Microsoft.EntityFrameworkCore;
using NouAssist.RequirementsManagement.ValidationVerification.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.RequirementsManagement.ValidationVerification.EntityFrameworkCore
{
    [ConnectionStringName(ValidationVerificationDbProperties.ConnectionStringName)]
    public interface IValidationVerificationDbContext : IEfCoreDbContext
    {
        // DbSet for each Aggregate Root in this module
        DbSet<RequirementReviewCycle> RequirementReviewCycles { get; }
        DbSet<TestCase> TestCases { get; }
    }
}
