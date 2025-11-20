using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.EntityFrameworkCore
{
    [ConnectionStringName(LegalComplianceDbProperties.ConnectionStringName)]
    public interface ILegalComplianceDbContext : IEfCoreDbContext
    {
        // DbSet properties for Aggregate Roots
        DbSet<IntellectualPropertyAsset> IntellectualPropertyAssets { get; }
        DbSet<LegalAgreement> LegalAgreements { get; }
        DbSet<ComplianceObligation> ComplianceObligations { get; }
        DbSet<CorporateRecord> CorporateRecords { get; }
    }
}
