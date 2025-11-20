using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Aggregates;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Entities;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Constants;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.EntityFrameworkCore
{
    /// <summary>
    /// DbContext interface for IRM module exposing DbSets for all aggregate roots
    /// </summary>
    [ConnectionStringName(IRMDbProperties.ConnectionStringName)]
    public interface IIRMDbContext : IEfCoreDbContext
    {
        // Aggregate Roots
        DbSet<InvestorProfile> InvestorProfiles { get; }
        DbSet<InvestorUpdate> InvestorUpdates { get; }
        DbSet<DueDiligenceDataRoom> DueDiligenceDataRooms { get; }
        DbSet<InvestorGroup> InvestorGroups { get; }
    }
}
