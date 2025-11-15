using Microsoft.EntityFrameworkCore;
using NouAssist.TaxManagement.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.TaxManagement.EntityFrameworkCore
{
    [ConnectionStringName(TaxManagementDbProperties.ConnectionStringName)]
    public class TaxManagementDbContext 
        : AbpDbContext<TaxManagementDbContext>, ITaxManagementDbContext
    {
        public DbSet<TaxReturn> TaxReturns { get; set; }
        public DbSet<TaxProfile> TaxProfiles { get; set; }

        public TaxManagementDbContext(DbContextOptions<TaxManagementDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureTaxManagement(this);
        }
    }
}
