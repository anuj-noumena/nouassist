using Microsoft.EntityFrameworkCore;
using NouAssist.FinancialManagement.InvestmentManagement.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.InvestmentManagement.EntityFrameworkCore
{
    [ConnectionStringName(InvestmentManagementDbProperties.ConnectionStringName)]
    public class InvestmentManagementDbContext 
        : AbpDbContext<InvestmentManagementDbContext>, IInvestmentManagementDbContext
    {
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<InvestmentGoal> InvestmentGoals { get; set; }

        public InvestmentManagementDbContext(DbContextOptions<InvestmentManagementDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureInvestmentManagement(this);
        }
    }
}
