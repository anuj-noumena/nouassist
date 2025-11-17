using Microsoft.EntityFrameworkCore;
using NouAssist.FinancialManagement.TaxManagement.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.TaxManagement.EntityFrameworkCore
{
    [ConnectionStringName(TaxManagementDbProperties.ConnectionStringName)]
    public interface ITaxManagementDbContext : IEfCoreDbContext
    {
        DbSet<TaxReturn> TaxReturns { get; }
        DbSet<TaxProfile> TaxProfiles { get; }
    }
}
