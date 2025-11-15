using Microsoft.EntityFrameworkCore;
using NouAssist.DebtManagement.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.DebtManagement.EntityFrameworkCore
{
    [ConnectionStringName(DebtManagementDbProperties.ConnectionStringName)]
    public class DebtManagementDbContext 
        : AbpDbContext<DebtManagementDbContext>, IDebtManagementDbContext
    {
        public DbSet<DebtAccount> DebtAccounts { get; set; }
        public DbSet<DebtPayoffPlan> DebtPayoffPlans { get; set; }

        public DebtManagementDbContext(DbContextOptions<DebtManagementDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureDebtManagement(this);
        }
    }
}
