using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.EntityFrameworkCore
{
    [ConnectionStringName(BusinessModelManagementDbProperties.ConnectionStringName)]
    public class BusinessModelManagementDbContext 
        : AbpDbContext<BusinessModelManagementDbContext>, IBusinessModelManagementDbContext
    {
        public DbSet<BusinessModelCanvas> BusinessModelCanvases { get; set; }
        public DbSet<RevenueModel> RevenueModels { get; set; }
        public DbSet<StrategicPivot> StrategicPivots { get; set; }

        public BusinessModelManagementDbContext(DbContextOptions<BusinessModelManagementDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ConfigureBusinessModelManagement(this);
        }
    }
}
