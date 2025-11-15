using Microsoft.EntityFrameworkCore;
using NouAssist.DebtManagement.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.DebtManagement.EntityFrameworkCore
{
    [ConnectionStringName(DebtManagementDbProperties.ConnectionStringName)]
    public interface IDebtManagementDbContext : IEfCoreDbContext
    {
        DbSet<DebtAccount> DebtAccounts { get; }
        DbSet<DebtPayoffPlan> DebtPayoffPlans { get; }
    }
}
