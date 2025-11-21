using Microsoft.EntityFrameworkCore;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Aggregates.Contract;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Aggregates.Portfolio;
using NouAssist.ContractPortfolioManagement.Aggregates.Contract;
using NouAssist.ContractPortfolioManagement.Aggregates.Portfolio;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.ContractPortfolioManagement.EntityFrameworkCore
{
    /// <summary>
    /// EF Core DBContext interface for ContractPortfolioManagement module
    /// </summary>
    [ConnectionStringName(ContractPortfolioManagementDbProperties.ConnectionStringName)]
    public interface IContractPortfolioManagementDbContext : IEfCoreDbContext
    {
        // DbSets for Aggregate Roots
        DbSet<ContractAggregate> Contracts { get; }
        DbSet<PortfolioAggregate> Portfolios { get; }
        DbSet<PortfolioRoadmapAggregate> PortfolioRoadmaps { get; }
    }
}
