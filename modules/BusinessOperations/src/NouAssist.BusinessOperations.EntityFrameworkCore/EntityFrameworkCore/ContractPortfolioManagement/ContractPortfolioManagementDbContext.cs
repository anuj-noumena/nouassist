using Microsoft.EntityFrameworkCore;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Aggregates.Contract;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Aggregates.Portfolio;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Entities.Contract;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Entities.Portfolio;
using NouAssist.ContractPortfolioManagement.Aggregates.Contract;
using NouAssist.ContractPortfolioManagement.Aggregates.Portfolio;
using NouAssist.ContractPortfolioManagement.Entities.Contract;
using NouAssist.ContractPortfolioManagement.Entities.Portfolio;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.ContractPortfolioManagement.EntityFrameworkCore
{
    /// <summary>
    /// EF Core DBContext for ContractPortfolioManagement module
    /// </summary>
    [ConnectionStringName(ContractPortfolioManagementDbProperties.ConnectionStringName)]
    public class ContractPortfolioManagementDbContext : 
        AbpDbContext<ContractPortfolioManagementDbContext>, 
        IContractPortfolioManagementDbContext
    {
        // Aggregate Roots
        public DbSet<ContractAggregate> Contracts { get; set; }
        public DbSet<PortfolioAggregate> Portfolios { get; set; }
        public DbSet<PortfolioRoadmapAggregate> PortfolioRoadmaps { get; set; }

        // Entities (optional - can be accessed through aggregates)
        public DbSet<ContractVersion> ContractVersions { get; set; }
        public DbSet<ContractClause> ContractClauses { get; set; }
        public DbSet<ContractParty> ContractParties { get; set; }
        public DbSet<ContractAmendment> ContractAmendments { get; set; }
        public DbSet<ContractAlert> ContractAlerts { get; set; }
        public DbSet<PortfolioItem> PortfolioItems { get; set; }
        public DbSet<PortfolioCategory> PortfolioCategories { get; set; }
        public DbSet<PortfolioRisk> PortfolioRisks { get; set; }
        public DbSet<PortfolioComplianceRecord> PortfolioComplianceRecords { get; set; }
        public DbSet<RoadmapMilestone> RoadmapMilestones { get; set; }
        public DbSet<CrossItemDependency> CrossItemDependencies { get; set; }

        public ContractPortfolioManagementDbContext(DbContextOptions<ContractPortfolioManagementDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // IMPORTANT: Call base first to apply ABP conventions
            base.OnModelCreating(builder);

            // Apply all configurations defined in this assembly using the extension method
            builder.ConfigureContractPortfolioManagement();
        }
    }
}
