using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Aggregates;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Entities;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Constants;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.EntityFrameworkCore
{
    /// <summary>
    /// DbContext implementation for IRM module
    /// </summary>
    [ConnectionStringName(IRMDbProperties.ConnectionStringName)]
    public class IRMDbContext : AbpDbContext<IRMDbContext>, IIRMDbContext
    {
        // Aggregate Roots
        public DbSet<InvestorProfile> InvestorProfiles { get; set; }
        public DbSet<InvestorUpdate> InvestorUpdates { get; set; }
        public DbSet<DueDiligenceDataRoom> DueDiligenceDataRooms { get; set; }
        public DbSet<InvestorGroup> InvestorGroups { get; set; }

        public IRMDbContext(DbContextOptions<IRMDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // IMPORTANT: Call base first to apply ABP conventions
            base.OnModelCreating(builder);

            // Apply all configurations defined in this assembly
            builder.ConfigureIRM();
        }
    }
}
