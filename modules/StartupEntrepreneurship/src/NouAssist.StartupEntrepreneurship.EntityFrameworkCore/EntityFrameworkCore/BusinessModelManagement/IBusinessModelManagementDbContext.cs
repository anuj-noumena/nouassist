using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.EntityFrameworkCore
{
    [ConnectionStringName(BusinessModelManagementDbProperties.ConnectionStringName)]
    public interface IBusinessModelManagementDbContext : IEfCoreDbContext
    {
        DbSet<BusinessModelCanvas> BusinessModelCanvases { get; }
        DbSet<RevenueModel> RevenueModels { get; }
        DbSet<StrategicPivot> StrategicPivots { get; }
    }
}
