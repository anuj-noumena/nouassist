using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.EntityFrameworkCore
{
    [ConnectionStringName(LegalComplianceDbProperties.ConnectionStringName)]
    public class LegalComplianceDbContext : AbpDbContext<LegalComplianceDbContext>, ILegalComplianceDbContext
    {
        // Implement DbSet properties from the interface
        public DbSet<IntellectualPropertyAsset> IntellectualPropertyAssets { get; set; }
        public DbSet<LegalAgreement> LegalAgreements { get; set; }
        public DbSet<ComplianceObligation> ComplianceObligations { get; set; }
        public DbSet<CorporateRecord> CorporateRecords { get; set; }

        public LegalComplianceDbContext(DbContextOptions<LegalComplianceDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Call base first to apply ABP conventions
            base.OnModelCreating(builder);

            // Apply configurations using the extension method
            builder.ConfigureLegalCompliance();
        }
    }
}
