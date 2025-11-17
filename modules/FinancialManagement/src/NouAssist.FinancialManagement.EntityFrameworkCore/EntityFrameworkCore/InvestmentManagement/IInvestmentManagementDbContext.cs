using Microsoft.EntityFrameworkCore;
using NouAssist.FinancialManagement.InvestmentManagement.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.InvestmentManagement.EntityFrameworkCore
{
    [ConnectionStringName(InvestmentManagementDbProperties.ConnectionStringName)]
    public interface IInvestmentManagementDbContext : IEfCoreDbContext
    {
        DbSet<Portfolio> Portfolios { get; }
        DbSet<InvestmentGoal> InvestmentGoals { get; }
    }
}
